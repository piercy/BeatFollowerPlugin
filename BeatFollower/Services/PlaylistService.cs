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
				if (Directory.Exists(PlaylistFolderPath))
				{
					_siraLog.Debug("Writing Playlist: " + PlaylistFolderPath + playlistFileName + " Length: " + playlistJson.Length);
					File.WriteAllText(PlaylistFolderPath + playlistFileName, playlistJson);
				}
			}));
		}

		public bool DoesPlaylistExist(string twitch, string playlistType = "Recommended")
		{
			var playlistFileName = $"BeatFollowerRecommended-{twitch}.json";
			var exists = File.Exists(PlaylistFolderPath + playlistFileName);
			_siraLog.Debug("Exists: " + exists + " " + PlaylistFolderPath + playlistFileName);
			return exists;
		}

		public int GetSongCount(string twitch, string playlistType = "Recommended")
		{
			var playlistFileName = $"BeatFollowerRecommended-{twitch}.json";
			var exists = File.Exists(PlaylistFolderPath + playlistFileName);
			if (exists)
			{
				var playlist = JsonConvert.DeserializeObject<Playlist>(File.ReadAllText(PlaylistFolderPath + playlistFileName));
				_siraLog.Debug(playlistFileName + " " + playlist.Songs.Count);
				return playlist.Songs.Count;
			}
			else
			{
				return 0;
			}
		}
	}
}