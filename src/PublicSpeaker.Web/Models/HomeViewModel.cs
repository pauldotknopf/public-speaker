using System;
using PublicSpeaker.Web.Models.Common;

namespace PublicSpeaker.Web.Models
{
	public class HomeViewModel
	{
		public TrackModel CurrentlyPlaying { get; set; }

		public List<TrackModel> Queue { get; set; }
	}
}

