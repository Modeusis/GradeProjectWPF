using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
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
using System.Windows.Media.Imaging;
using Microsoft.Extensions.Options;

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
        public DbSet<Discipline> Disciplines { get; set; } = null!;
        public DbSet<Statistics> Statistics { get; set; } = null!;
        public ApplicationContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
            
            //Users.Load();
            //Players.Load();
            //Matches.Load();
            //Teams.Load();
            //Tournaments.Load();
            //TournamentComments.Load();
            //Disciplines.Load();
            //Statistics.Load();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tournamentdb;Username=postgres;Password=123456");
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
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
            modelBuilder.Entity<Discipline>()
                .HasKey(c => c.DisciplineId);
            modelBuilder.Entity<Statistics>()
                .HasKey(c => c.StatisticId);

            modelBuilder.Entity<Statistics>()
                .HasOne(s => s.Player)
                .WithMany(p => p.Statistics)
                .HasForeignKey(s => s.PlayerId);
                
            modelBuilder.Entity<Statistics>()
                .HasMany(s => s.Matches)
                .WithMany(m => m.Statistics)
                .UsingEntity<Dictionary<string, object>>(
                    "MatchStatistic",
                    j => j.HasOne<Match>().WithMany().HasForeignKey("MatchId"),
                    j => j.HasOne<Statistics>().WithMany().HasForeignKey("StatisticId")
                    );
            modelBuilder.Entity<Statistics>()
                .HasMany(s => s.Teams)
                .WithMany(t => t.Statistics)
                .UsingEntity<Dictionary<string, object>>(
                    "TeamStatistic",
                    j => j.HasOne<Team>().WithMany().HasForeignKey("TeamId"),
                    j => j.HasOne<Statistics>().WithMany().HasForeignKey("StatisticId")
                    );

            modelBuilder.Entity<User>()
                .HasMany(f => f.Comments)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.Author);
            modelBuilder.Entity<Team>()
                .HasMany(a => a.Users)
                .WithOne(f => f.Team)
                .HasForeignKey(f => f.FavTeamId);
            modelBuilder.Entity<Tournament>()
                .HasMany(a => a.Matches)
                .WithOne(f => f.Tournament)
                .HasForeignKey(f => f.TournamentId);
            modelBuilder.Entity<Tournament>()
                .HasMany(a => a.TournamentComments)
                .WithOne(f => f.Tournament)
                .HasForeignKey(f => f.TournamentId);
            modelBuilder.Entity<Tournament>()
                .HasMany(a => a.Users)
                .WithOne(f => f.Tournament)
                .HasForeignKey(f => f.FavTournamentId);
            modelBuilder.Entity<Discipline>()
                .HasMany(a => a.Tournaments)
                .WithOne(f => f.Discipline)
                .HasForeignKey(f => f.DisciplineId);
            modelBuilder.Entity<Team>()
                .HasMany(a => a.Players)
                .WithOne(f => f.Team)
                .HasForeignKey(f => f.CurTeamId);
            modelBuilder.Entity<Team>()
                .HasMany(a => a.Tournaments)
                .WithMany(f => f.Teams)
                .UsingEntity<Dictionary<string, object>>(
                    "TournamentTeam",
                    j => j.HasOne<Tournament>().WithMany().HasForeignKey("TournamentId"),
                    j => j.HasOne<Team>().WithMany().HasForeignKey("TeamId")
                    );
            modelBuilder.Entity<Match>()
                .HasOne(m => m.FirstTeam)
                .WithMany(a => a.MatchesAsFirstTeam)
                .HasForeignKey(m => m.FirstParticipantId);
            modelBuilder.Entity<Match>()
                .HasOne(m => m.SecondTeam)
                .WithMany(a => a.MatchesAsSecondTeam)
                .HasForeignKey(m => m.SecondParticipantId);
            modelBuilder.Entity<Player>()
                .HasMany(a => a.Users)
                .WithOne(f => f.Player)
                .HasForeignKey(f => f.FavPlayerId);
            modelBuilder.Entity<User>()
                .HasData(
                    new User() { UserId = 1, Username = "Modeus", FavTeamId = 1, FavPlayerId = 26, FavTournamentId = 1, Description = "Creator of this App", Login = "1", Password = PasswordHasher.HashPassword("1"), IsLogined=false, IsAdmin = true, Logo = ImageConverter.StandardUserIcon, HeaderImg = ImageConverter.StandardHeaderIcon, CreatedAt = DateTime.UtcNow},
                    new User() { UserId = 2, Username = "ModeusGuest", FavPlayerId = 15, FavTournamentId = 1, Description = "Guest of this App", Login = "2", Password = PasswordHasher.HashPassword("2"), IsLogined=false, IsAdmin = false, Logo = ImageConverter.StandardUserIcon, HeaderImg = ImageConverter.StandardHeaderIcon, CreatedAt = DateTime.UtcNow}
                );
            modelBuilder.Entity<Discipline>()
                .HasData(
                    new Discipline() { DisciplineId = 1, DisciplineName = "CS2", Description = "One of the most popular tactical shooter in the world", CreatedAt = DateTime.Parse("27.09.2023").ToUniversalTime() }
                );
            modelBuilder.Entity<Team>().HasData(
                new Team { TeamId = 1, WorldRanking = 2, TeamName = "Natus Vincere", TeamLogo = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Teams/Natus Vincere.png"), CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Team { TeamId = 2, WorldRanking = 1, TeamName = "G2", TeamLogo = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Teams/G2.png"), CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Team { TeamId = 3, WorldRanking = 3, TeamName = "Team Spirit", TeamLogo = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Teams/Spirit.png"), CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Team { TeamId = 4, WorldRanking = 4, TeamName = "Vitality", TeamLogo = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Teams/Vitality.png"), CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Team { TeamId = 5, WorldRanking = 5, TeamName = "MOUZ", TeamLogo = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Teams/MOUZ.png"), CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Team { TeamId = 6, WorldRanking = 6, TeamName = "Falcons", TeamLogo = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Teams/Falcons.png"), CreatedAt = DateTime.UtcNow, UpdatedAt = null }
            );

            modelBuilder.Entity<Player>().HasData(
                new Player { PlayerId = 1, PlayerName = "jL", PlayerRealName = "Justinas Lekavicius", Position = "Rifler", BirthDayDate = new DateTime(1998, 4, 10).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/jL.png"), CurTeamId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 2, PlayerName = "b1t", PlayerRealName = "Valerii Vakhovskyi", Position = "Rifler", BirthDayDate = new DateTime(2003, 1, 5).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/b1t.png"), CurTeamId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 3, PlayerName = "w0nderful", PlayerRealName = "Aleksandr Skrypin", Position = "AWPer", BirthDayDate = new DateTime(2004, 7, 26).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/w0nderful.png"), CurTeamId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 4, PlayerName = "iM", PlayerRealName = "Mihai Ivan", Position = "Rifler", BirthDayDate = new DateTime(1999, 2, 3).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/iM.png"), CurTeamId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 5, PlayerName = "Aleksib", PlayerRealName = "Aleksi Virolainen", Position = "In-Game Leader", BirthDayDate = new DateTime(1997, 3, 30).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/Aleksib.png"), CurTeamId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = null },

                new Player { PlayerId = 6, PlayerName = "NiKo", PlayerRealName = "Nikola Kovac", Position = "Rifler", BirthDayDate = new DateTime(1997, 2, 16).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/NiKo.png"), CurTeamId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 7, PlayerName = "huNter-", PlayerRealName = "Nemanja Kovac", Position = "Rifler", BirthDayDate = new DateTime(1995, 1, 9).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/huNter-.png"), CurTeamId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 8, PlayerName = "m0NESY", PlayerRealName = "Ilya Osipov", Position = "AWPer", BirthDayDate = new DateTime(2005, 5, 1).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/m0NESY.png"), CurTeamId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 9, PlayerName = "malbsMd", PlayerRealName = "Mario Samayoa", Position = "Rifler", BirthDayDate = new DateTime(2001, 10, 24).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/malbsMd.png"), CurTeamId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 10, PlayerName = "Snax", PlayerRealName = "Janusz Pogorzelski", Position = "Lurker", BirthDayDate = new DateTime(1993, 7, 5).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/Snax.png"), CurTeamId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = null },

                new Player { PlayerId = 11, PlayerName = "chopper", PlayerRealName = "Leonid Vishnyakov", Position = "In-Game Leader", BirthDayDate = new DateTime(1997, 2, 18).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/chopper.png"), CurTeamId = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 12, PlayerName = "sh1ro", PlayerRealName = "Dmitry Sokolov", Position = "AWPer", BirthDayDate = new DateTime(2001, 6, 15).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/sh1ro.png"), CurTeamId = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 13, PlayerName = "magixx", PlayerRealName = "Boris Vorobiev", Position = "Rifler", BirthDayDate = new DateTime(2003, 5, 10).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/magixx.png"), CurTeamId = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 14, PlayerName = "zont1x", PlayerRealName = "Aleksandr Zhdanov", Position = "Support", BirthDayDate = new DateTime(2004, 3, 25).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/zont1x.png"), CurTeamId = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 15, PlayerName = "donk", PlayerRealName = "Danil Kryshkovets", Position = "Entry Fragger", BirthDayDate = new DateTime(2006, 7, 12).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/donk.png"), CurTeamId = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = null },

                new Player { PlayerId = 16, PlayerName = "ZywOo", PlayerRealName = "Mathieu Herbaut", Position = "AWPer", BirthDayDate = new DateTime(2000, 11, 9).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/ZywOo.png"), CurTeamId = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 17, PlayerName = "mezii", PlayerRealName = "William Merriman", Position = "Rifler", BirthDayDate = new DateTime(1999, 7, 22).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/mezii.png"), CurTeamId = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 18, PlayerName = "Spinx", PlayerRealName = "Lotan Giladi", Position = "Rifler", BirthDayDate = new DateTime(2000, 8, 22).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/Spinx.png"), CurTeamId = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 19, PlayerName = "apEX", PlayerRealName = "Dan Madesclaire", Position = "In-Game Leader", BirthDayDate = new DateTime(1993, 2, 22).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/apEX.png"), CurTeamId = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 20, PlayerName = "flameZ", PlayerRealName = "Shahar Shushan", Position = "Entry Fragger", BirthDayDate = new DateTime(2003, 8, 5).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/flameZ.png"), CurTeamId = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = null },

                new Player { PlayerId = 21, PlayerName = "Brollan", PlayerRealName = "Ludvig Brolin", Position = "Rifler", BirthDayDate = new DateTime(2002, 6, 17).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/Brollan.png"), CurTeamId = 5, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 22, PlayerName = "xertioN", PlayerRealName = "Dorian Berman", Position = "Rifler", BirthDayDate = new DateTime(2003, 3, 1).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/xertioN.png"), CurTeamId = 5, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 23, PlayerName = "torzsi", PlayerRealName = "Ádám Torzsás", Position = "AWPer", BirthDayDate = new DateTime(2002, 10, 18).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/torzsi.png"), CurTeamId = 5, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 24, PlayerName = "siuhy", PlayerRealName = "Kamil Szkaradek", Position = "In-Game Leader", BirthDayDate = new DateTime(2002, 11, 5).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/siuhy.png"), CurTeamId = 5, CreatedAt = DateTime.UtcNow, UpdatedAt = null },
                new Player { PlayerId = 25, PlayerName = "Jimpphat", PlayerRealName = "Jimi Salo", Position = "Rifler", BirthDayDate = new DateTime(2005, 6, 3).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/Jimpphat.png"), CurTeamId = 5, CreatedAt = DateTime.UtcNow, UpdatedAt = null },

                new Player { PlayerId = 26, PlayerName = "s1mple", PlayerRealName = "Oleksandr Kostyliev", Position = "AWPer", BirthDayDate = new DateTime(1997, 10, 2).ToUniversalTime(), PlayerImg = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Players/s1mple.png"), CurTeamId = 6, CreatedAt = DateTime.UtcNow, UpdatedAt = null }
            );
            modelBuilder.Entity<Tournament>().HasData(
                new Tournament
                {
                    TournamentId = 1,
                    TournamentName = "PGL RMR Winter Cup",
                    DisciplineId = 1,
                    Img = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/Tournaments/pglRMR.png"),
                    CreatedAt = DateTime.UtcNow,
                    StartDate = DateTime.UtcNow
                });
            modelBuilder.Entity<Match>().HasData(
                new Match() { MatchId = 1, TournamentId = 1, FirstParticipantId = 1, SecondParticipantId = 2, Status = true, WinnerId = 1, ScoreFirstTeam = 13, ScoreSecondTeam = 9, MatchTime = DateTime.Parse("15.12.2024").ToUniversalTime() },
                new Match() { MatchId = 2, TournamentId = 1, FirstParticipantId = 3, SecondParticipantId = 4, Status = true, WinnerId = 3, ScoreFirstTeam = 13, ScoreSecondTeam = 11, MatchTime = DateTime.Parse("15.12.2024").ToUniversalTime() },
                new Match() { MatchId = 3, TournamentId = 1, FirstParticipantId = 1, SecondParticipantId = 3, Status = false, MatchTime = DateTime.Parse("16.12.2024").ToUniversalTime() }
            );
        }
    }
}
