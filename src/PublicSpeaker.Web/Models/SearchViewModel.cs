using System;
using PublicSpeaker.Web.Models.Common;

namespace PublicSpeaker.Web.Models
{
	public class SearchViewModel
	{
		public List<TrackModel> Tracks { get; set; }

		public string Query { get; set; }
	}
}

