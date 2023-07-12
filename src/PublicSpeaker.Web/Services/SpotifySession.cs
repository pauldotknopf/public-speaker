using System;
namespace PublicSpeaker.Web.Services
{
	public class SpotifySession
	{
		public string AccessToken { get; private set; }

		public void SetAccessToken(string accessToken)
		{
			AccessToken = accessToken;
		}
	}
}

