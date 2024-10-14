using System;

namespace BeatFollower.Services
{
	internal class BeatmapCollector : IDisposable
	{
		private readonly LastBeatmapManager _lastBeatmapManager;
		private readonly BeatmapLevel _lastBeatmapLevel;
		private readonly BeatmapKey _lastBeatmapKey;

		public BeatmapCollector(LastBeatmapManager lastBeatmapManager, BeatmapLevel lastBeatmapLevel, BeatmapKey lastBeatmapKey)
		{
			_lastBeatmapManager = lastBeatmapManager;
			_lastBeatmapLevel = lastBeatmapLevel;
			_lastBeatmapKey = lastBeatmapKey;

		}

		public void Dispose()
		{
			_lastBeatmapManager.LastBeatmapLevel = _lastBeatmapLevel;
			_lastBeatmapManager.LastBeatmapKey = _lastBeatmapKey;
		}
	}
}