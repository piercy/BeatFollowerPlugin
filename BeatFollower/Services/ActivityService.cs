using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Threading.Tasks;
using BeatFollower.Models;
using BeatFollower.Utilities;
using SiraUtil.Logging;
using Zenject;

namespace BeatFollower.Services
{
	internal class ActivityService : IInitializable, IDisposable
	{
		private readonly SiraLog _siraLog;
		private readonly RequestService _requestService;
		private readonly StandardLevelScenesTransitionSetupDataSO _standardLevelScenesTransitionSetupDataSo;
		private readonly LastBeatmapManager _lastBeatmapManager;

		public ActivityService(SiraLog siraLog, RequestService requestService, StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupDataSo, LastBeatmapManager lastBeatmapManager)
		{
			_siraLog = siraLog;
			_requestService = requestService;
			_standardLevelScenesTransitionSetupDataSo = standardLevelScenesTransitionSetupDataSo;
			_lastBeatmapManager = lastBeatmapManager;
		}

		public void Initialize()
		{
			_standardLevelScenesTransitionSetupDataSo.didFinishEvent += DidFinishLevelHandler;
		}

		public void Dispose()
		{
			_standardLevelScenesTransitionSetupDataSo.didFinishEvent -= DidFinishLevelHandler;
		}

		public async Task SubmitActivity(StandardLevelScenesTransitionSetupDataSO setupData, LevelCompletionResults levelCompletionResults)
		{
			try
			{
				var levelEndState = levelCompletionResults.levelEndStateType;
				var difficultyBeatmap = setupData.difficultyBeatmap;
				var currentSong = difficultyBeatmap.level;

				if (currentSong.IsWip())
				{
					_siraLog.Debug("WIP so not sending activity.");
					return;
				}


				var beatMapData = await difficultyBeatmap.GetBeatmapDataAsync(setupData.environmentInfo);
				var maxScore = ScoreModel.ComputeMaxMultipliedScoreForBeatmap(beatMapData);

					//difficultyBeatmap.beatmapData.cuttableNotesCount);
				var normalizedAcc = ((double)levelCompletionResults.multipliedScore / maxScore) * 100;

				_siraLog.Debug("levelCompletionResults.energy " + levelCompletionResults.energy);

				var activity = new Activity
				{
					NoFail = (levelCompletionResults.gameplayModifiers.noFailOn0Energy && Math.Abs(levelCompletionResults.energy) <= 0),
					WipMap = currentSong.levelID.EndsWith("WIP"),
					PracticeMode = setupData.practiceSettings != null,
					Difficulty = difficultyBeatmap.difficulty,
					EndType = levelEndState,
					SongName = currentSong.songName,
					SongSubName = currentSong.songSubName,
					SongAuthorName = currentSong.songAuthorName,
					LevelAuthorName = currentSong.levelAuthorName,
					FullCombo = levelCompletionResults.fullCombo,
					EndSongTime = levelCompletionResults.endSongTime.ToString("##.##", new CultureInfo("en-US")),
					Accuracy = normalizedAcc.ToString("##.##", new CultureInfo("en-US")),
					Is90 = difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName.Equals("90Degree")
				};

				if (activity.Accuracy == "NaN")
				{
					_siraLog.Debug("modified score: " + levelCompletionResults.multipliedScore);
					_siraLog.Debug("maxScore: " + maxScore);
					_siraLog.Debug("acc: " + normalizedAcc);
				}

				// cant be 90 and 360 at the same time, so if we've already detected 90, cant be 360, this is a best guess effort
				activity.Is360 = (difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName.Equals("360Degree") ||
				                  beatMapData.spawnRotationEventsCount > 0) && !activity.Is90;
				activity.OneSaber = difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName.Equals("OneSaber");

				if (currentSong is CustomBeatmapLevel customBeatmapLevel)
				{
					activity.Hash = SongCore.Utilities.Hashing.GetCustomLevelHash(customBeatmapLevel);
				}
				else
				{
					_siraLog.Debug("OST Level");
					activity.Hash = currentSong.levelID;
					activity.Ost = true;
				}

				var httpResponse = await _requestService.Post($"activity/", activity);

				if (httpResponse != null)
				{
					if (!httpResponse.Successful)
					{
						throw new Exception(httpResponse.Code + await httpResponse.Error());
					}

				}
			}
			catch (Exception ex)
			{
				_siraLog.Error(ex);
			}
		}

		public async Task SubmitRecommendation(IDifficultyBeatmap beatmap, string listId = "")
		{
			_siraLog.Debug("Submitting Level");
			try
			{
				var level = beatmap.level;
				if (level.IsWip())
				{
					_siraLog.Debug("WIP so not sending recommendation.");
					return;
				}

				var recommendation = new Recommendation
				{
					Hash = SongCore.Utilities.Hashing.GetCustomLevelHash(level as CustomPreviewBeatmapLevel),
					SongName = level.songName,
					SongSubName = level.songSubName,
					SongAuthorName = level.songAuthorName,
					LevelAuthorName = level.levelAuthorName,
					Difficulty = beatmap.difficulty,
					Characteristic = beatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName
				};


				var httpResponse = await _requestService.Post($"recommendation/" + listId, recommendation);

				if (httpResponse != null)
				{
					if (!httpResponse.Successful)
					{
						throw new Exception(httpResponse.Code + await httpResponse.Error());
					}

				}
			}
			catch (Exception ex)
			{
				_siraLog.Error(ex);
			}
		}

		private void DidFinishLevelHandler(StandardLevelScenesTransitionSetupDataSO setupData, LevelCompletionResults completionResults)
		{
			SubmitActivity(setupData, completionResults);
		}
	}
}