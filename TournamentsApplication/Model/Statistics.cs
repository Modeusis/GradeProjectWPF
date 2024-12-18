using System.Collections.Generic;
using TournamentsApplication.Utility;

namespace TournamentsApplication.Model
{
    public class Statistics : ViewModelBase
    {
        private int statisticId;
        private int matchId;
        private int teamId;
        private int playerId;
        private string playerKD;
        private int playerKills;
        private int playerDeaths;
        private int playerAssists;

        public int StatisticId
        {
            get => statisticId;
            set { statisticId = value; OnPropertyChanged(nameof(StatisticId)); }
        }

        public int MatchId
        {
            get => matchId;
            set { matchId = value; OnPropertyChanged(nameof(MatchId)); }
        }

        public int TeamId
        {
            get => teamId;
            set { teamId = value; OnPropertyChanged(nameof(TeamId)); }
        }

        public int PlayerId
        {
            get => playerId;
            set { playerId = value; OnPropertyChanged(nameof(PlayerId)); }
        }

        public string PlayerKD
        {
            get => playerKD;
            set { playerKD = value; OnPropertyChanged(nameof(PlayerKD)); }
        }

        public int PlayerKills
        {
            get => playerKills;
            set { playerKills = value; OnPropertyChanged(nameof(PlayerKills)); }
        }

        public int PlayerDeaths
        {
            get => playerDeaths;
            set { playerDeaths = value; OnPropertyChanged(nameof(PlayerDeaths)); }
        }

        public int PlayerAssists
        {
            get => playerAssists;
            set { playerAssists = value; OnPropertyChanged(nameof(PlayerAssists)); }
        }

        public virtual ICollection<MatchStatistic> Matches { get; set; } = new List<MatchStatistic>();
        public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
        public virtual Player Player { get; set; }
    }
}
