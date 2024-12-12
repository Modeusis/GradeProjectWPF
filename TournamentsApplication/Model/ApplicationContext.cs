using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System;
using System.Drawing;
using System.IO;
using TournamentsApplication.Utility;

namespace TournamentsApplication.Model
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<Match> Matches { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<Tournament> Tournaments { get; set; } = null!;
        public DbSet<TournamentComment> TournamentComments { get; set; } = null!;
        public DbSet<TournamentTeam> TournamentTeams { get; set; } = null!;
        public DbSet<Discipline> Disciplines { get; set; } = null!;
        public DbSet<FavTeam> FavTeams { get; set; } = null!;
        public DbSet<FavPlayer> FavPlayers { get; set; } = null!;
        public DbSet<FavTournament> FavTournaments { get; set; } = null!;

        public ApplicationContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();

            Users.Load();
            Players.Load();
            Matches.Load();
            Teams.Load();
            Tournaments.Load();
            TournamentComments.Load();
            TournamentTeams.Load();
            Disciplines.Load();
            FavTournaments.Load();
            FavPlayers.Load();
            FavTeams.Load();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tournamentdb;Username=postgres;Password=123456");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(c => c.UserId);
            modelBuilder.Entity<Player>()
                .HasKey(c => c.PlayerId);
            modelBuilder.Entity<Match>()
                .HasKey(c => c.MatchId);
            modelBuilder.Entity<Team>()
                .HasKey(c => c.TeamId);
            modelBuilder.Entity<Tournament>()
                .HasKey(c => c.TournamentId);
            modelBuilder.Entity<TournamentComment>()
                .HasKey(c => c.CommentId);
            modelBuilder.Entity<TournamentTeam>()
                .HasKey(c => c.TournamentTeamId);
            modelBuilder.Entity<Discipline>()
                .HasKey(c => c.DisciplineId);
            modelBuilder.Entity<FavTeam>()
                .HasKey(c => c.FavTeamId);
            modelBuilder.Entity<FavPlayer>()
                .HasKey(c => c.FavPlayerId);
            modelBuilder.Entity<FavTournament>()
                .HasKey(c => c.FavTournamentId);

            modelBuilder.Entity<User>()
                .HasMany(f => f.Comments)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.Author);
            modelBuilder.Entity<User>()
                .HasMany(a => a.FavPlayers)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId);
            modelBuilder.Entity<User>()
                .HasMany(a => a.FavTournaments)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId);
            modelBuilder.Entity<User>()
                .HasMany(a => a.FavTeams)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId);
            modelBuilder.Entity<Tournament>()
                .HasMany(a => a.TournamentTeams)
                .WithOne(f => f.Tournament)
                .HasForeignKey(f => f.TournamentId);
            modelBuilder.Entity<Tournament>()
                .HasMany(a => a.Matches)
                .WithOne(f => f.Tournament)
                .HasForeignKey(f => f.TournamentId);
            modelBuilder.Entity<Tournament>()
                .HasMany(a => a.TournamentComments)
                .WithOne(f => f.Tournament)
                .HasForeignKey(f => f.TournamentId);
            modelBuilder.Entity<Tournament>()
                .HasMany(a => a.FavTournaments)
                .WithOne(f => f.Tournament)
                .HasForeignKey(f => f.TournamentId);
            modelBuilder.Entity<Discipline>()
                .HasMany(a => a.Tournaments)
                .WithOne(f => f.Discipline)
                .HasForeignKey(f => f.DisciplineId);
            modelBuilder.Entity<Team>()
                .HasMany(a => a.Players)
                .WithOne(f => f.Team)
                .HasForeignKey(f => f.CurTeamId);
            modelBuilder.Entity<Team>()
                .HasMany(a => a.TournamentTeams)
                .WithOne(f => f.Team)
                .HasForeignKey(f => f.TeamId);
            modelBuilder.Entity<Match>()
                .HasOne(m => m.FirstTeam)
                .WithMany()
                .HasForeignKey(m => m.FirstParticipantId);
            modelBuilder.Entity<Match>()
                .HasOne(m => m.SecondTeam)
                .WithMany()
                .HasForeignKey(m => m.SecondParticipantId);
            modelBuilder.Entity<Team>()
                .HasMany(a => a.FavTeams)
                .WithOne(f => f.Team)
                .HasForeignKey(f => f.TeamId);
            modelBuilder.Entity<Player>()
                .HasMany(a => a.FavPlayers)
                .WithOne(f => f.Player)
                .HasForeignKey(f => f.PlayerId);

            modelBuilder.Entity<User>()
                .HasData(
                    new User() { UserId = 1, Username = "Modeus", Description = "Creator of this App", Login = "Modeus", Password = PasswordHasher.HashPassword("123456"), IsLogined=false, IsAdmin = true, CreatedAt = DateTime.Now.ToUniversalTime()},
                    new User() { UserId = 2, Username = "ModeusGuest", Description = "Guest of this App", Login = "ModeusGuest", Password = PasswordHasher.HashPassword("1111"), IsLogined=false, IsAdmin = false, CreatedAt = DateTime.Now.ToUniversalTime()}
                );
        }
    }
}
