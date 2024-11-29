namespace MusicPlayer.Models
{
    public class SongEconomy
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public int SongClicks { get; set;}
        public double SongPlayPrice { get; set; }
        public double SongMoneyMade { get; set;}

    }
}
