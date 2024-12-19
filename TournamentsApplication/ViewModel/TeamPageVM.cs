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
        public bool IsAdmin => UserService.Instance.Admin;
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



        private bool isChanging;
        public bool IsChanging
        {
            get => isChanging; 
            set { isChanging = value; OnPropertyChanged(); }
        }

        private string newTeamName;
        public string NewTeamName
        {
            get { return newTeamName; }
            set { newTeamName = value; OnPropertyChanged(); }
        }

        private byte[]? newTeamImg;
        public byte[]? NewTeamImg
        {
            get { return newTeamImg; }
            set { newTeamImg = value; OnPropertyChanged(); }
        }
        private string newWorldRank;
        public string NewWorldRank
        {
            get { return newWorldRank; }
            set { newWorldRank = value; OnPropertyChanged(); }
        }

        private RelayCommand? editChangeTeamCommand;
        public RelayCommand? EditChangeTeamCommand
        {
            get
            {
                return editChangeTeamCommand ??= new RelayCommand((obj) =>
                {
                    NewTeamImg = TeamLogo;
                    IsChanging = true;
                });
            }
        }
        private RelayCommand selectTeamLogoCommand;
        public RelayCommand SelectTeamLogoCommand
        {
            get
            {
                return selectTeamLogoCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        var openedImage = ImageConverter.OpenAndLoadImage();
                        if (openedImage is byte[] imgByte)
                        {
                            NewTeamImg = imgByte;
                        }
                    }
                    catch (Exception ex)
                    {
                        StatusService.Instance.SetStatusMessage(ex.Message);
                    }
                });
            }
        }
        private RelayCommand? changeTeamConfirmCommand;
        public RelayCommand? ChangeTeamConfirmCommand
        {
            get
            {
                return changeTeamConfirmCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        Team tmpTeam = Team;
                        if (NewTeamImg == TeamLogo && string.IsNullOrEmpty(NewTeamName) && string.IsNullOrEmpty(NewWorldRank))
                        {
                            throw new Exception("Nothing is changed");
                        }
                        int WorldRank;
                        if (!string.IsNullOrEmpty(NewWorldRank))
                        {
                            if (!int.TryParse(NewWorldRank, out WorldRank))
                            {
                                throw new Exception("Incorrect world rank");
                            }
                            if (WorldRank <= 0)
                            {
                                throw new Exception("Only POSITIVE rank");
                            }
                            int amountOfTeams = uow.Teams.GetAll().Count();
                            if (WorldRank > amountOfTeams + 1)
                            {
                                WorldRank = amountOfTeams + 1;
                            }
                            if (WorldRank - WorldRanking < 0)
                            {
                                if (uow.Teams.GetAll().Where(a => a.WorldRanking < WorldRanking && a.WorldRanking >= WorldRank).Count() > 0)
                                {
                                    foreach (var team in uow.Teams.GetAll().Where(a => a.WorldRanking < WorldRanking && a.WorldRanking >= WorldRank))
                                    {
                                        team.WorldRanking++;
                                        uow.Teams.Update(team);
                                    }
                                }
                            }
                            else if (WorldRank == WorldRanking)
                            {
                                throw new Exception("Cant have same rank");
                            }
                            else
                            { 
                                if (uow.Teams.GetAll().Where(a => a.WorldRanking > WorldRanking && a.WorldRanking <= WorldRank).Count() > 0)
                                {
                                    foreach (var team in uow.Teams.GetAll().Where(a => a.WorldRanking > WorldRanking && a.WorldRanking <= WorldRank))
                                    {
                                        team.WorldRanking--;
                                        uow.Teams.Update(team);
                                    }
                                }
                            }

                            tmpTeam.WorldRanking = WorldRank;
                        }
                        if (!string.IsNullOrEmpty(NewTeamName))
                        {
                            tmpTeam.TeamName = NewTeamName;
                        }                       
                        if (NewTeamImg != null && NewTeamImg != TeamLogo)
                        {
                            tmpTeam.TeamLogo = NewTeamImg;
                        }
                        
                        uow.Teams.Update(tmpTeam);
                        uow.Save();
                        ContentNavigationService.Instance.SwitchCurrentContentView(new TeamPageView(uow.Teams.GetById(Team.TeamId)));
                        StatusService.Instance.SetStatusMessage("Updated");
                    }
                    catch (Exception e)
                    {
                        StatusService.Instance.SetStatusMessage(e.Message);
                    }
                });
            }
        }
        private RelayCommand? dismissTeamConfirmCommand;
        public RelayCommand? DismissTeamConfirmCommand
        {
            get
            {
                return dismissTeamConfirmCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        IsChanging = false;
                        NewTeamImg = null;
                        NewTeamName = string.Empty;
                        NewWorldRank = string.Empty;

                        StatusService.Instance.SetStatusMessage("Changes dismissed successfully");
                    }
                    catch (Exception e)
                    {
                        StatusService.Instance.SetStatusMessage(e.Message);
                    }
                });
            }
        }



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
            if (team.Tournaments.Select(tt => tt.Tournament).Count() > 0)
            {
                IsHaveTeamTournaments = true;
                var tournaments = uow.TournamentTeams.GetAll().Where(tt => tt.TeamId == Team.TeamId).Select(tt => tt.Tournament).Take(3);
                TeamTournaments = new ObservableCollection<Tournament>(tournaments);
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
            OnPropertyChanged(nameof(IsAdmin));
        }
    }
}
