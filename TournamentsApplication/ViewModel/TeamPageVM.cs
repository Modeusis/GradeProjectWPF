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
        public User? CurrentUser => UserService.Instance.CurrentUser;
        public bool IsLogin => UserService.Instance.Login;
        private Team team;
        private string teamName;
        private int worldRanking;
        private byte[]? teamLogo;
        private byte[] favoriteIcon;
        private bool isHaveTeamPlayers = false;
        private bool isHaveTeamTournaments = false;
        private bool isHaveTeamMatches = false;
        private bool isFavoriteTeam = false;

        public bool IsFavoriteTeam
        {
            get => isFavoriteTeam;
            set { isFavoriteTeam = value; OnPropertyChanged(); }
        }
        public byte[] FavoriteIcon
        {
            get { return favoriteIcon; }
            set { favoriteIcon = value; OnPropertyChanged(); }
        }
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
        private RelayCommand? toMatchPageCommand;
        public RelayCommand? ToMatchPageCommand
        {
            get
            {
                return toMatchPageCommand ??
                    (toMatchPageCommand = new RelayCommand((obj) =>
                    {
                        if (obj is Match match)
                        {
                            ContentNavigationService.Instance.SwitchCurrentContentView(new MatchPageView(match));
                        }
                    }));
            }
        }
        private RelayCommand? toTournamentPageCommand;
        public RelayCommand? ToTournamentPageCommand
        {
            get
            {
                return toTournamentPageCommand ??
                    (toTournamentPageCommand = new RelayCommand((obj) =>
                    {
                        if (obj is Tournament tournament) ContentNavigationService.Instance.SwitchCurrentContentView(new TournamentPageView(tournament));
                    }));
            }
        }
        private RelayCommand? changeFavoriteCommand;
        public RelayCommand ChangeFavoriteCommand
        {
            get
            {
                return changeFavoriteCommand ??
                    (changeFavoriteCommand = new RelayCommand((obj) =>
                    {
                        try
                        {
                            if (IsFavoriteTeam == true)
                            {
                                User user = CurrentUser;
                                user.FavTeamId = null;
                                IsFavoriteTeam = false;
                                StatusService.Instance.SetStatusMessage("Unfavorite");
                                FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starEmpty.png");
                                uow.Users.Update(user);
                                uow.Save();
                                UserService.Instance.RenewCurrentUser(user);
                            }
                            else
                            {
                                User user = CurrentUser;
                                user.FavTeamId = Team.TeamId;
                                IsFavoriteTeam = true;
                                StatusService.Instance.SetStatusMessage("Favorite");
                                FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starFill.png");
                                uow.Users.Update(user);
                                uow.Save();
                                UserService.Instance.RenewCurrentUser(user);
                            }
                        }
                        catch (Exception ex)
                        {

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
            FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starEmpty.png");
            if (CurrentUser !=null && CurrentUser.FavTeamId == Team.TeamId)
            {
                IsFavoriteTeam = true;
                FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starFill.png");
            }
            TeamName = Team.TeamName;
            TeamLogo = Team.TeamLogo;
            WorldRanking = Team.WorldRanking;
            if (team.Players.Count() > 0)
            {
                IsHaveTeamPlayers = true;
                TeamPlayers = new ObservableCollection<Player>(team.Players);
            }
            if (team.MatchesAsFirstTeam.Count() > 0 || team.MatchesAsSecondTeam.Count() > 0)
            {
                IsHaveTeamMatches = true;
                var matches = uow.Matches.GetAll().Where(a => a.FirstParticipantId == team.TeamId || a.SecondParticipantId == team.TeamId).Take(3);
                TeamMatches = new ObservableCollection<Match>(matches);
            }
            if (uow.Tournaments.GetAll().Where(a => a.Teams.Any(a => a.TeamId == team.TeamId)).Count() > 0)
            {
                IsHaveTeamTournaments = true;
                var tournaments = uow.Tournaments.GetAll().Select(a => a.Teams.Where(a => a.TeamId == team.TeamId).Take(3));
                TeamTournaments = new ObservableCollection<Tournament>(team.Tournaments);
            }
        }
        private void OnContentChanged()
        {
            OnPropertyChanged(nameof(CurrentContentView));
            
        }
        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(CurrentUser));
            OnPropertyChanged(nameof(IsLogin));
        }
    }
}
