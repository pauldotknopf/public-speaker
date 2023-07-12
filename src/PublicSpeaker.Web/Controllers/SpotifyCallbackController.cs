using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PublicSpeaker.Web.Services;
using SpotifyAPI.Web;

namespace PublicSpeaker.Web.Controllers
{
	public class SpotifyCallbackController : Controller
	{
        private SpotifySession _spotifySession;
        private SpotifyConfig _spotifyConfig;

        public SpotifyCallbackController(IOptions<SpotifyConfig> spotifyConfig,
			SpotifySession spotifySession)
		{
			_spotifyConfig = spotifyConfig.Value;
            _spotifySession = spotifySession;
        }

		public async Task<ActionResult> Index(string code)
		{
            var redirectUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/spotifycallback";

            var response = await new OAuthClient()
				.RequestToken(new AuthorizationCodeTokenRequest(_spotifyConfig.ClientId, _spotifyConfig.ClientSecret, code, new Uri(redirectUri)));

			_spotifySession.SetAccessToken(response.AccessToken);

			return Redirect("/");
        }
	}
}

