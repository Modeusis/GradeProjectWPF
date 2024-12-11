using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public ApplicationContext()
        {
            Database.EnsureCreated();

            Users.Load();
            Players.Load();
            Matches.Load();
            Teams.Load();
            Tournaments.Load();
            TournamentComments.Load();
            TournamentTeams.Load();
            Disciplines.Load();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tournamentdb;Username=postgres;Password=123456");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(f => f.Comments)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.Author);
            modelBuilder.Entity<Tournament>()
                .HasMany(f => f.TournamentComments)
                .WithOne(a => a.Tournament)
                .HasForeignKey(a => a.TournamentId);
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
            modelBuilder.Entity<Team>()
                .HasMany(a => a.Matches)
                .WithOne(f => f.FirstTeam)
                .HasForeignKey(f => f.FirstParticipantId);
            modelBuilder.Entity<Team>()
                .HasMany(a => a.Matches)
                .WithOne(f => f.SecondTeam)
                .HasForeignKey(f => f.SecondParticipantId);

        }
    }
}
