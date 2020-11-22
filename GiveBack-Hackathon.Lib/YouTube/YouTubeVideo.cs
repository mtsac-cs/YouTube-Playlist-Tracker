using System;

namespace GiveBack_Hackathon.Lib.YouTube
{
    public class YoutubeVideo
    {
        public string Title { get; set; }
        public string ParentPlaylist { get; set; }
        public int IndexInPlaylist { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsDeleted { get; set; }
    }
}