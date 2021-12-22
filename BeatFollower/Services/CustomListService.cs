using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeatFollower.Models;
using Newtonsoft.Json;
using SiraUtil.Logging;

namespace BeatFollower.Services
{
	internal class CustomListService
	{
		private readonly SiraLog _siraLog;
		private readonly RequestService _requestService;

		private List<CustomList> _customLists = new();

		public CustomListService(SiraLog siraLog, RequestService requestService)
		{
			_siraLog = siraLog;
			_requestService = requestService;
		}

		public async Task GetCustomLists(Action<List<CustomList>> callback)
		{
			_siraLog.Debug("Getting Custom Lists..");
			if (_customLists.Count > 0)
			{
				_siraLog.Debug("Custom Lists: " + _customLists.Count);
				callback.Invoke(_customLists);
			}
			else
			{
				var httpResponse = await _requestService.Get("lists/get");

				if (httpResponse != null)
				{
					var json = await httpResponse.ReadAsStringAsync();

					var customlistsResponse = JsonConvert.DeserializeObject<List<CustomList>>(json);
					_customLists = customlistsResponse;
					_siraLog.Debug("Custom Lists Fetched: " + _customLists.Count);
					callback.Invoke(_customLists);
				}
			}
		}
	}
}