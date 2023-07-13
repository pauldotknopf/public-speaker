using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PublicSpeaker.Web.Models;
using PublicSpeaker.Web.Services;
using SpotifyAPI.Web;

namespace PublicSpeaker.Web.Controllers;

public class HomeController : Controller
{
    private ILogger<HomeController> _logger;
    private SpotifyClient _spotifyClient;

    public HomeController(ILogger<HomeController> logger,
        SpotifyClient spotifyClient)
    {
        _logger = logger;
        _spotifyClient = spotifyClient;
    }

    public async Task<IActionResult> Index()
    {
        var model = new HomeViewModel();

        var queue = await _spotifyClient.Player.GetQueue();

        if(queue.CurrentlyPlaying != null)
        {
            model.CurrentlyPlaying = Models.Common.TrackModel.FromPlayableItem(queue.CurrentlyPlaying);
        }

        if(queue.Queue != null)
        {
            model.Queue = queue.Queue.Select(x => Models.Common.TrackModel.FromPlayableItem(x)).ToList();
        }
        else
        {
            model.Queue = new List<Models.Common.TrackModel>();
        }

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
