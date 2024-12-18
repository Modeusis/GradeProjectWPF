using System;
using TournamentsApplication.Utility;

namespace TournamentsApplication.Model
{
    public class MatchStatistic : ViewModelBase
    {
        private int matchStatisticId;
        private int matchId;
        private int statisticId;

        public int MatchStatisticId
        {
            get => matchStatisticId;
            set { matchStatisticId = value; OnPropertyChanged(); }
        }
        public int MatchId
        {
            get => matchId;
            set { matchId = value; OnPropertyChanged(); }
        }


        public int StatisticId
        {
            get => statisticId;
            set { statisticId = value; OnPropertyChanged(); }
        }

        public virtual Match Match { get; set; }
        public virtual Statistics Statistic { get; set; }
    }
}
