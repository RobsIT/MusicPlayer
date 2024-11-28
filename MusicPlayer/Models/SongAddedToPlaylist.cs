namespace MusicPlayer.Models
{
    public class SongAddedToPlaylist
    {
        public int Id { get; set; }
        public int SongFileId { get; set; }
        public int PlaylistId { get; set;}
    }
}
