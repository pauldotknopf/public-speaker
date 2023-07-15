using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PublicSpeaker.Web.Services;
using SpotifyAPI.Web;

namespace PublicSpeaker.Web.Controllers
{
	public class AuthController : Controller
	{
        private SpotifySession _spotifySession;
        private SpotifyConfig _spotifyConfig;

        public AuthController(IOptions<SpotifyConfig> spotifyConfig,
			SpotifySession spotifySession)
		{
			_spotifyConfig = spotifyConfig.Value;
            _spotifySession = spotifySession;
        }

		public async Task<ActionResult> Callback(string code)
		{
            var redirectUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/auth/callback";

            var response = await new OAuthClient()
				.RequestToken(new AuthorizationCodeTokenRequest(_spotifyConfig.ClientId, _spotifyConfig.ClientSecret, code, new Uri(redirectUri)));

			var user = await (new SpotifyClient(response.AccessToken).UserProfile.Current());
			if(user.Id != "31jbzwiulqccqk2hpvkh3qodfbf4")
			{
				// someone else trying to use my shit?
				return Content("invalid user");
			}

			var refreshAt = DateTime.Now.Add(TimeSpan.FromSeconds(response.ExpiresIn)).Subtract(TimeSpan.FromMinutes(1));

			_spotifySession.SetSessionInfo(new SpotifySessionInfo(response.AccessToken, response.RefreshToken, refreshAt));

			return Redirect("/");
        }

		public IActionResult Logout()
		{
			_spotifySession.SetSessionInfo(null);
			return Content("logged out");
		}
	}
}

