using System;
using Microsoft.AspNetCore.Mvc;
using PublicSpeaker.Web.Models;
using SpotifyAPI.Web;

namespace PublicSpeaker.Web.Controllers
{
	public class QueueController : Controller
	{
        private SpotifyClient _spotifyClient;

        public QueueController(SpotifyClient spotifyClient)
		{
            _spotifyClient = spotifyClient;
        }

		[HttpGet]
		public async Task<ActionResult> Search(string query)
		{
			var model = new SearchViewModel { Query = query };

			if (!string.IsNullOrEmpty(model.Query))
			{
				var result = await _spotifyClient.Search.Item(new SearchRequest(SearchRequest.Types.Track, model.Query) { Limit = 30 });
				model.Tracks = result.Tracks.Items.Select(x => Models.Common.TrackModel.FromPlayableItem(x)).ToList();
			}
			else
			{
				model.Tracks = new List<Models.Common.TrackModel>();
			}

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Search(SearchViewModel model)
		{
            if (!string.IsNullOrEmpty(model.Query))
            {
                var result = await _spotifyClient.Search.Item(new SearchRequest(SearchRequest.Types.Track, model.Query) { Limit = 30 });
                model.Tracks = result.Tracks.Items.Select(x => Models.Common.TrackModel.FromPlayableItem(x)).ToList();
            }
            else
            {
                model.Tracks = new List<Models.Common.TrackModel>();
            }

			return View(model);
        }

		[HttpPost]
		public async Task<ActionResult> AddTrack(string uri, string query)
		{
			var result = await _spotifyClient.Player.AddToQueue(new PlayerAddToQueueRequest(uri));
			if(!result)
			{
				TempData["error"] = "There was an error adding a track to the queue.";
				return RedirectToAction("Search", new { query });
			}
			else
			{
				TempData["success"] = "The track was added to the queue!";
				return RedirectToAction("Index", "Home");
			}
		}
	}
}

