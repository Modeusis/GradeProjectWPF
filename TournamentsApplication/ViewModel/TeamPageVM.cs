using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TournamentsApplication.Model;
using TournamentsApplication.Utility;

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
        private bool isHereTeamPlayers = false;
        private bool isHereTeamTournaments = false;
        private bool isHereTeamMatches = false;
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
        public bool IsHereTeamPlayers
        {
            get { return isHereTeamPlayers; }
            set { isHereTeamPlayers = value; OnPropertyChanged(); }
        }
        public bool IsHereTeamTournaments
        {
            get { return isHereTeamTournaments; }
            set { isHereTeamTournaments = value; OnPropertyChanged(); }
        }
        public bool IsHereTeamMatches
        {
            get { return isHereTeamMatches; }
            set { isHereTeamMatches = value; OnPropertyChanged(); }
        }
        private ObservableCollection<Tournament> TeamTournaments { get; set; }
        private ObservableCollection<Match> TeamMatches { get; set; }
        private ObservableCollection<Player> TeamPlayers { get; set; }
        public TeamPageVM(Team team)
        {
            uow = new UnitOfWork(new ApplicationContext());

            Team = team;
            TeamName = Team.TeamName;
            TeamLogo = Team.TeamLogo;
            WorldRanking = Team.WorldRanking;
            if (Team.Players.Count > 0)
            {
                IsHereTeamPlayers = true;
                TeamPlayers = new ObservableCollection<Player>(team.Players);
            }
            if (Team.Matches.Count > 0)
            {
                IsHereTeamMatches = true;
                TeamMatches = new ObservableCollection<Match>(team.Matches);
            }
            if (Team.TournamentTeams.Where(a => a.TeamId == Team.TeamId).Count() > 0)
            {
                IsHereTeamTournaments = true;
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
