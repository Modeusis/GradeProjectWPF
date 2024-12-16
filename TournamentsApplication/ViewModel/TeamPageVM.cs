using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TournamentsApplication.Model;
using TournamentsApplication.Utility;
using TournamentsApplication.View;

namespace TournamentsApplication.ViewModel
{
    internal class TeamPageVM : ViewModelBase
    {
        private UnitOfWork uow;
        public UserControl? CurrentContentView => ContentNavigationService.Instance.CurrentContentView;
        private Team team;
        private string teamName;
        private int worldRanking;
        private byte[]? teamLogo;
        private bool isHaveTeamPlayers = false;
        private bool isHaveTeamTournaments = false;
        private bool isHaveTeamMatches = false;
        private RelayCommand? itemClickCommand;
        public RelayCommand? ItemClickCommand
        {
            get
            {
                return itemClickCommand ??
                    (itemClickCommand = new RelayCommand((obj) =>
                    {
                        if (obj is Player plr)
                        {
                            ContentNavigationService.Instance.SwitchCurrentContentView(new PlayerPageView(plr));
                        }

                    }));
            }
        }
        public Team Team
        {
            get { return team; }
            set { team = value; OnPropertyChanged(); }
        }
        public string TeamName
        {
            get { return teamName; }
            set { teamName = value; OnPropertyChanged(); }
        }
        public int WorldRanking
        {
            get { return worldRanking; }
            set { worldRanking = value; OnPropertyChanged(); }
        }
        public byte[]? TeamLogo
        {
            get { return teamLogo; }
            set { teamLogo = value; OnPropertyChanged(); }
        }
        public bool IsHaveTeamPlayers
        {
            get { return isHaveTeamPlayers; }
            set { isHaveTeamPlayers = value; OnPropertyChanged(); }
        }
        public bool IsHaveTeamTournaments
        {
            get { return isHaveTeamTournaments; }
            set { isHaveTeamTournaments = value; OnPropertyChanged(); }
        }
        public bool IsHaveTeamMatches
        {
            get { return isHaveTeamMatches; }
            set { isHaveTeamMatches = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Tournament> TeamTournaments { get; set; }
        public ObservableCollection<Match> TeamMatches { get; set; }
        public ObservableCollection<Player> TeamPlayers { get; set; }


        public TeamPageVM(Team team)
        {
            uow = new UnitOfWork(new ApplicationContext());

            Team = team;
            TeamName = Team.TeamName;
            TeamLogo = Team.TeamLogo;
            WorldRanking = Team.WorldRanking;
            if (team.Players != null)
            {
                IsHaveTeamPlayers = true;
                TeamPlayers = new ObservableCollection<Player>(team.Players);
            }
            if (team.Matches != null)
            {
                IsHaveTeamMatches = true;
                TeamMatches = new ObservableCollection<Match>(team.Matches);
            }
            if (team.TournamentTeams.Where(a => a.TeamId == Team.TeamId) != null)
            {
                IsHaveTeamTournaments = true;
                var TournamentsWithTeam = uow.Tournaments.GetAll().Where(a => a.TournamentTeams.Any(b => b.Team == team));
                TeamTournaments = new ObservableCollection<Tournament>(TournamentsWithTeam);
            }
        }
        private void OnContentChanged()
        {
            OnPropertyChanged(nameof(CurrentContentView));
        }
    }
}
