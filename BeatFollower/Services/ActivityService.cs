using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Threading.Tasks;
using BeatFollower.Models;
using BeatFollower.Utilities;
using IPA.Logging;
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
		private readonly PlayerDataModel _playerDataModel;

		public ActivityService(SiraLog siraLog, RequestService requestService, StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupDataSo, LastBeatmapManager lastBeatmapManager, PlayerDataModel playerDataModel)
		{
			_siraLog = siraLog;
			_requestService = requestService;
			_standardLevelScenesTransitionSetupDataSo = standardLevelScenesTransitionSetupDataSo;
			_lastBeatmapManager = lastBeatmapManager;
			_playerDataModel = playerDataModel;
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
				var beatmapKey = setupData.beatmapKey;
				var beatmapLevel = setupData.beatmapLevel;

				if (beatmapLevel.IsWip())
				{
					_siraLog.Debug("WIP so not sending activity.");
					return;
				}

				var maxScore = ScoreModel.ComputeMaxMultipliedScoreForBeatmap(setupData.transformedBeatmapData);

				var normalizedAcc = ((double)levelCompletionResults.multipliedScore / maxScore) * 100;

				_siraLog.Debug("levelCompletionResults.energy " + levelCompletionResults.energy);

				var activity = new Activity
				{
					NoFail = (levelCompletionResults.gameplayModifiers.noFailOn0Energy && Math.Abs(levelCompletionResults.energy) <= 0),
					WipMap = beatmapLevel.levelID.EndsWith("WIP"),
					PracticeMode = setupData.practiceSettings != null,
					Difficulty = beatmapKey.difficulty,
					EndType = levelEndState,
					SongName = beatmapLevel.songName,
					SongSubName = beatmapLevel.songSubName,
					SongAuthorName = beatmapLevel.songAuthorName,
					LevelAuthorName = string.Join(" ", beatmapLevel.allMappers),
					FullCombo = levelCompletionResults.fullCombo,
					EndSongTime = levelCompletionResults.endSongTime.ToString("##.##", new CultureInfo("en-US")),
					Accuracy = normalizedAcc.ToString("##.##", new CultureInfo("en-US")),
					Is90 = beatmapKey.beatmapCharacteristic.serializedName.Equals("90Degree")
				};

				if (activity.Accuracy == "NaN")
				{
					_siraLog.Debug("modified score: " + levelCompletionResults.multipliedScore);
					_siraLog.Debug("maxScore: " + maxScore);
					_siraLog.Debug("acc: " + normalizedAcc);
				}

				// cant be 90 and 360 at the same time, so if we've already detected 90, cant be 360, this is a best guess effort
				activity.Is360 = (beatmapKey.beatmapCharacteristic.serializedName.Equals("360Degree") ||
				                  setupData.transformedBeatmapData.spawnRotationEventsCount > 0) && !activity.Is90;
				activity.OneSaber = beatmapKey.beatmapCharacteristic.serializedName.Equals("OneSaber");


				if (beatmapLevel.levelID.ToUpper().StartsWith("CUSTOM_LEVEL_"))
				{

					activity.Hash = SongCore.Utilities.Hashing.GetCustomLevelHash(beatmapLevel);
				}
				else
				{
					_siraLog.Debug("OST Level");
					activity.Hash = beatmapLevel.levelID;
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

		public async Task SubmitRecommendation(BeatmapLevel beatmapLevel, BeatmapKey beatmapKey, string listId = "")
		{
			_siraLog.Debug("Submitting Level");
			try
			{
				if (beatmapLevel.IsWip())
				{
					_siraLog.Debug("WIP so not sending recommendation.");
					return;
				}

				var recommendation = new Recommendation
				{
					SongName = beatmapLevel.songName,
					SongSubName = beatmapLevel.songSubName,
					SongAuthorName = beatmapLevel.songAuthorName,
					LevelAuthorName = string.Join(" ", beatmapLevel.allMappers),
					Difficulty = beatmapKey.difficulty,
					Characteristic = beatmapKey.beatmapCharacteristic.serializedName
				};

				if (beatmapLevel.levelID.ToUpper().StartsWith("CUSTOM_LEVEL_"))
				{
					recommendation.Hash = SongCore.Utilities.Hashing.GetCustomLevelHash(beatmapLevel);
				}
				else
				{
					_siraLog.Debug("OST Level");
					recommendation.Hash = beatmapLevel.levelID;
				}



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