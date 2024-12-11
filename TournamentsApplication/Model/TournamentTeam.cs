using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentsApplication.Utility;

namespace TournamentsApplication.Model
{
    internal class TournamentTeam : ViewModelBase
    {
        private int tournament_id;
        private int team_id;

        public int TournamentId
        {
            get { return tournament_id; }
            set { tournament_id = value; OnPropertyChanged(nameof(TournamentId)); }
        }
        public int TeamId
        {
            get { return team_id; }
            set { team_id = value; OnPropertyChanged(nameof(TeamId)); }
        }

        public Tournament Tournament { get; set; }
        public Team Team { get; set; }
    }
}
