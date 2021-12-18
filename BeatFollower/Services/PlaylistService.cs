using System.IO;
using BeatFollower.Models;
using Newtonsoft.Json;
using SiraUtil.Logging;

namespace BeatFollower.Services
{
	internal class PlaylistService
	{
		private readonly SiraLog _siraLog;
		private readonly RequestService _requestService;

		private static string PlaylistFolderPath => Path.Combine(IPA.Utilities.UnityGame.InstallPath, "Playlists");

		public PlaylistService(SiraLog siraLog, RequestService requestService)
		{
			_siraLog = siraLog;
			_requestService = requestService;
		}

		public void DownloadPlaylist(string twitch, string playlistType = "Recommended")
		{
			_siraLog.Debug("Requesting playlist");

			SharedCoroutineStarter.instance.StartCoroutine(_requestService.Get($"feed/playlist/{playlistType}/{twitch}/BeatFollower{playlistType}-{twitch}.json", playlistJson =>
			{
				var playlistFileName = $"BeatFollowerRecommended-{twitch}.json";
				Directory.CreateDirectory(PlaylistFolderPath);

				var playListPath = Path.Combine(PlaylistFolderPath, playlistFileName);
				_siraLog.Debug("Writing Playlist: " + playListPath + " Length: " + playlistJson.Length);
				File.WriteAllText(playListPath, playlistJson);
			}));
		}

		public bool DoesPlaylistExist(string twitch, string playlistType = "Recommended")
		{
			if (!Directory.Exists(PlaylistFolderPath))
			{
				return false;
			}

			var playListPath = Path.Combine(PlaylistFolderPath, $"BeatFollowerRecommended-{twitch}.json");
			var exists = File.Exists(playListPath);
			_siraLog.Debug("Exists: " + exists + " " + playListPath);
			return exists;
		}

		public int GetSongCount(string twitch, string playlistType = "Recommended")
		{
			if (!DoesPlaylistExist(twitch))
			{
				return 0;
			}

			var playlistFileName = $"BeatFollowerRecommended-{twitch}.json";
			var playListPath = Path.Combine(PlaylistFolderPath, playlistFileName);
			var exists = File.Exists(playListPath);
			if (exists)
			{
				var playlist = JsonConvert.DeserializeObject<Playlist>(File.ReadAllText(playListPath));
				_siraLog.Debug(playlistFileName + " " + playlist.Songs.Count);
				return playlist.Songs.Count;
			}

			return 0;
		}
	}
}