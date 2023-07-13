using System;
using SpotifyAPI.Web;

namespace PublicSpeaker.Web.Models.Common
{
	public class TrackModel
	{
		public string Uri { get; set; }

		public string Title { get; set; }

		public string[] Artists { get; set; }

		public string AlbumImageUrl { get; set; }

		public string PreviewAudioUrl { get; set; }

		public static TrackModel FromPlayableItem(IPlayableItem playableItem)
		{
			switch(playableItem)
			{
				case FullTrack t:
					return new TrackModel
					{
						Uri = t.Uri,
						Title = t.Name,
						Artists = t.Artists.Select(x => x.Name).ToArray(),
						AlbumImageUrl = t.Album.Images.First().Url,
						PreviewAudioUrl = t.PreviewUrl
					};
				default:
					throw new NotSupportedException();
			}
		}
	}
}

