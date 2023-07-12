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
        var result = await _spotifyClient.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest {});

        var queue = await _spotifyClient.Player.GetQueue();

        return View();
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
