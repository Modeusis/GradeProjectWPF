using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentsApplication.Utility;

namespace TournamentsApplication.Model
{
    public class TournamentTeam : ViewModelBase
    {
        private int tournamentTeamId;
        public int TournamentTeamId
        {
            get => tournamentTeamId;
            set {tournamentTeamId = value; OnPropertyChanged(); }
        }
        private int teamId;
        private int tournamentId;
        public int TeamId
        {
            get => teamId; 
            set { teamId = value; OnPropertyChanged(); }
        }
        public int TournamentId
        {
            get => tournamentId;
            set { tournamentId = value; OnPropertyChanged(); }
        }

        public virtual Team Team { get; set; }
        public virtual Tournament Tournament { get; set; }
    }
}
