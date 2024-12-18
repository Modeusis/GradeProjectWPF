﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TournamentsApplication.Model;
using TournamentsApplication.ViewModel;

namespace TournamentsApplication.Utility
{
    internal class UnitOfWork
    {
        private readonly DbContext _context;
        public Repository<User> Users { get; set; }
        public Repository<Discipline> Disciplines { get; set; }
        public Repository<Model.Match> Matches { get; set; }
        public Repository<Player> Players { get; set; }
        public Repository<Team> Teams { get; set; }
        public Repository<Tournament> Tournaments { get; set; }
        public Repository<TournamentComment> TournamentComments { get; set; }
        public Repository<Statistics> Statistics { get; set; }
        public Repository<MatchStatistic> MatchStatistics { get; set; }
        public Repository<MatchComment> MatchComments { get; set; }
        public Repository<TournamentTeam> TournamentTeams { get; set; }


        public UnitOfWork(DbContext context)
        {
            _context = context;

            Users = new Repository<User>(_context);
            Disciplines = new Repository<Discipline>(_context);
            Matches = new Repository<Model.Match>(_context);
            Players = new Repository<Player>(_context);
            Teams = new Repository<Team>(_context);
            Tournaments = new Repository<Tournament>(_context);
            TournamentComments = new Repository<TournamentComment>(_context);
            MatchComments = new Repository<MatchComment>(_context);
            TournamentTeams = new Repository<TournamentTeam>(_context);
            Statistics = new Repository<Statistics>(_context);
            MatchStatistics = new Repository<MatchStatistic>(_context);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }
    }
}
