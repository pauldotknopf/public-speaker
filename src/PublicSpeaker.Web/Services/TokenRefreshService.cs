using System;
using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Http;

namespace PublicSpeaker.Web.Services
{
	public class TokenRefreshService : BackgroundService, IDisposable
	{
        private ILogger<TokenRefreshService> _logger;
        private SpotifySession _spotifySession;
        private SpotifyConfig _spotifyConfig;

        public TokenRefreshService(ILogger<TokenRefreshService> logger,
            SpotifySession spotifySession,
            IOptions<SpotifyConfig> spotifyConfig)
        {
            _logger = logger;
            _spotifySession = spotifySession;
            _spotifyConfig = spotifyConfig.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new(TimeSpan.FromSeconds(30));

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await DoWork();
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Refresh token service is stopping.");
            }
        }

        // Could also be a async method, that can be awaited in ExecuteAsync above
        private async Task DoWork()
        {
            var sessionInfo = _spotifySession.GetSessionInfo();

            if(sessionInfo == null)
            {
                return;
            }

            if (DateTimeOffset.Now <= sessionInfo.RefreshAt)
            {
                return;
            }

            _logger.LogInformation("getting refresh token...");

            var response = await new OAuthClient().RequestToken
            (
                new AuthorizationCodeRefreshRequest(_spotifyConfig.ClientId, _spotifyConfig.ClientSecret, sessionInfo.RefreshToken)
            );

            var refreshAt = DateTimeOffset.Now.Add(TimeSpan.FromSeconds(response.ExpiresIn)).Subtract(TimeSpan.FromMinutes(1));

            _spotifySession.SetSessionInfo(new SpotifySessionInfo(response.AccessToken, response.RefreshToken ?? sessionInfo.RefreshToken, refreshAt));
        }
    }
}

