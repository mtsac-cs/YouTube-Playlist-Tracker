namespace YouTube_Playlist_Tracker.Lib.YouTube
{
    public class VideoInfo
    {
        public int IndexInPlaylist { get; set; }
        public string ParentPlaylist { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPrivate { get; set; }
    }
}