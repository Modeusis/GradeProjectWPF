﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentsApplication.Utility;

namespace TournamentsApplication.Model
{
    internal class Team : ViewModelBase
    {
        private int team_id;
        private string team_name;
        private byte[] team_logo;
        private DateTime created_at;
        private DateTime updated_at;

        public int TeamId
        {
            get { return team_id; }
            set { team_id = value; OnPropertyChanged(nameof(TeamId)); }
        }
        public string TeamName
        {
            get { return team_name; }
            set { team_name = value; OnPropertyChanged(nameof(TeamName)); }
        }
        public byte[] TeamLogo
        {
            get { return team_logo; }
            set { team_logo = value; OnPropertyChanged(nameof(TeamLogo)); }
        }
        public DateTime CreatedAt
        {
            get { return created_at; }
            set { created_at = value; OnPropertyChanged(nameof(CreatedAt)); }
        }
        public DateTime UpdatedAt
        {
            get { return updated_at; }
            set { updated_at = value; OnPropertyChanged(nameof(UpdatedAt)); }
        }
        public ICollection<Player> Players { get; set; }
        public ICollection<TournamentTeam> TournamentTeams { get; set; }
        public ICollection<Match> Matches { get; set; }
        public ICollection<FavTeam> FavTeams { get; set; }
    }
}
