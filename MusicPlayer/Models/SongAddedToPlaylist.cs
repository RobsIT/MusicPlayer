namespace MusicPlayer.Models
{
    public class SongAddedToPlaylist
    {
        public int Id { get; set; }
        public string SongFileId { get; set; }
        public string PlaylistId { get; set;}
    }
}
