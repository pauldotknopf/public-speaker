using System;
using System.Globalization;
using Microsoft.Extensions.Options;
using PublicSpeaker.Web.Services;
using SpotifyAPI.Web;

namespace PublicSpeaker.Web.Middleware
{
	public class SpotifyAuthMiddleware
	{
        private readonly RequestDelegate _next;
        private readonly SpotifySession _spotifySession;
        private readonly SpotifyConfig _spotifyConfig;

        public SpotifyAuthMiddleware(RequestDelegate next,
            SpotifySession spotifySession,
            IOptions<SpotifyConfig> spotifyConfig)
        {
            _spotifySession = spotifySession;
            _spotifyConfig = spotifyConfig.Value;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (string.IsNullOrEmpty(_spotifySession.AccessToken)
                && !context.Request.Path.ToString().ToLower().Equals("/spotifycallback"))
            {
                var redirectUri = $"{context.Request.Scheme}://{context.Request.Host.Value}/spotifycallback";
                var loginRequest = new LoginRequest(
                    new Uri(redirectUri),
                    _spotifyConfig.ClientId,
                    LoginRequest.ResponseType.Code)
                {
                    Scope = new[]
                    {
                        Scopes.UserModifyPlaybackState,
                        Scopes.UserReadPlaybackState,
                        Scopes.UserReadCurrentlyPlaying,
                        Scopes.UserReadRecentlyPlayed,
                        Scopes.UserModifyPlaybackState
                    }
                };
                var uri = loginRequest.ToUri();
                context.Response.Redirect(uri.ToString());
                return;
            }

            await _next(context);
        }
    }
}

