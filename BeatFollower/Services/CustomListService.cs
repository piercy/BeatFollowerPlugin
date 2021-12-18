using System;
using System.Collections.Generic;
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

	    public void GetCustomLists(Action<List<CustomList>> callback)
        {
	        _siraLog.Debug("Getting Custom Lists..");
            if (_customLists.Count > 0)
            {
                _siraLog.Debug("Custom Lists: " + _customLists.Count);
                callback.Invoke(_customLists);
            }
            else
            {
                SharedCoroutineStarter.instance.StartCoroutine(_requestService.Get("lists/get", response =>
                {
                    var customlistsResponse = JsonConvert.DeserializeObject<List<CustomList>>(response);
                    _customLists = customlistsResponse;
                    _siraLog.Debug("Custom Lists Fetched: " + _customLists.Count);
                    callback.Invoke(_customLists);
                }));
            }
        }
    }
}