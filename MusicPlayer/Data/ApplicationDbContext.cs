using Microsoft.EntityFrameworkCore;
using MusicPlayer.Models;

namespace MusicPlayer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Song> AllSongs { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<SongAddedToPlaylist> SongsAddedToPlaylists { get; set; }
        public DbSet<SongEconomy> SongsEconomies { get; set; }
        
        protected override async void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
