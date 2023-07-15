using System;
namespace PublicSpeaker.Web.Services
{
	public class SpotifySessionInfo
	{
		public SpotifySessionInfo(string accessToken, string refreshToken, DateTimeOffset refreshAt)
		{
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            RefreshAt = refreshAt;
        }

        public string AccessToken { get; }

        public string RefreshToken { get; }

        public DateTimeOffset RefreshAt { get; }
    }
}

