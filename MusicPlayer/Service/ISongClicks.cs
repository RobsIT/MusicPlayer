namespace MusicPlayer.Service
{
    public interface ISongClicks
    {
        Task IncrementSongClicksAsync(int songId);
    }
}
