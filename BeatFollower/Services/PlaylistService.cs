using System;
using System.IO;
using System.Threading.Tasks;
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

		public async Task DownloadPlaylist(string twitch, string playlistType = "Recommended")
		{
			_siraLog.Debug("Requesting playlist");


			var httpResponse = await _requestService.Get($"feed/playlist/{playlistType}/{twitch}/BeatFollower{playlistType}-{twitch}.json");

			if (httpResponse != null)
			{
				if (httpResponse.Successful)
				{

					try
					{
						var playlistManager = BeatSaberPlaylistsLib.PlaylistManager.DefaultManager;
						var playlist = playlistManager.DefaultHandler.Deserialize(await httpResponse.ReadAsStreamAsync());
						playlistManager.StorePlaylist(playlist);
					}
					catch (Exception e)
					{
						_siraLog.Info("Before Error");
						_siraLog.Error(e);
					}
				}
				else
				{
					_siraLog.Error(await httpResponse.Error());
				}
			}
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

		public Stream GenerateStreamFromString(string s)
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(s);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}
	}
}