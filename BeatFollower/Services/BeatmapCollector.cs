using System;

namespace BeatFollower.Services
{
	internal class BeatmapCollector : IDisposable
	{
		private readonly LastBeatmapManager _lastBeatmapManager;
		private readonly IDifficultyBeatmap _difficultyBeatmap;

		public BeatmapCollector(LastBeatmapManager lastBeatmapManager, IDifficultyBeatmap difficultyBeatmap)
		{
			_lastBeatmapManager = lastBeatmapManager;
			_difficultyBeatmap = difficultyBeatmap;
		}

		public void Dispose()
		{
			_lastBeatmapManager.LastBeatmap = _difficultyBeatmap;
		}
	}
}