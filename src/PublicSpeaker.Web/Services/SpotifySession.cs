using System;
namespace PublicSpeaker.Web.Services
{
	public class SpotifySession
	{
		private SpotifySessionInfo _spotifySessionInfo;
		private object _lock = new object();

		public void SetSessionInfo(SpotifySessionInfo sessionInfo)
		{
			lock(_lock)
			{
				_spotifySessionInfo = sessionInfo;
			}
		}

		public SpotifySessionInfo GetSessionInfo()
		{
			lock(_lock)
			{
				return _spotifySessionInfo;
			}
		}
	}
}

