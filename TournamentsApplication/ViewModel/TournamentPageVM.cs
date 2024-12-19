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
using Microsoft.EntityFrameworkCore;

namespace TournamentsApplication.ViewModel
{
    internal class TournamentPageVM : ViewModelBase
    {
        private UnitOfWork uow;
        public UserControl? CurrentContentView => ContentNavigationService.Instance.CurrentContentView;
        public User? CurrentUser => UserService.Instance.CurrentUser;
        public bool IsLogin => UserService.Instance.Login;
        public bool IsAdmin => UserService.Instance.Admin;
        private Tournament showedTournament;
        private Team? teamToRemove;
        private Team? teamToAdd;
        private string? newTournamentName;
        private string tournamentName;
        private string commentContent;
        private bool isFavoriteTournament = false;
        private byte[] favoriteIcon;
        private byte[] tournamentIcon;
        private byte[] tmpTournamentIcon;
        private bool isChanging = false;
        private bool isHereCommentsPage = false;
        private bool isHereMatchesPage = false;
        private int currentCommentsPage = 1;
        private int currentMatchesPage = 1;
        private int amountOfCommentsPages = 1;
        private int amountOfMatchesPages = 1;
        public bool IsHereCommentsPage
        {
            get => isHereCommentsPage;
            set { isHereCommentsPage = value; OnPropertyChanged(); }
        }
        public string? NewTournamentName
        {
            get => newTournamentName;
            set { newTournamentName = value; OnPropertyChanged(); }
        }
        public bool IsHereMatchesPage
        {
            get => isHereMatchesPage;
            set { isHereMatchesPage = value; OnPropertyChanged(); }
        }
        public bool IsChanging
        {
            get => isChanging;
            set { isChanging = value; OnPropertyChanged(); }
        }
        public int CurrentCommentsPage
        {
            get => currentCommentsPage;
            set { currentCommentsPage = value; OnPropertyChanged(); }
        }
        public int CurrentMatchesPage
        {
            get => currentMatchesPage;
            set { currentMatchesPage = value; OnPropertyChanged(); }
        }
        public int AmountOfCommentsPages
        {
            get => amountOfCommentsPages;
            set { amountOfCommentsPages = value; OnPropertyChanged(); }
        }
        public int AmountOfMatchesPages
        {
            get => amountOfMatchesPages;
            set { amountOfMatchesPages = value; OnPropertyChanged(); }
        }
        public ObservableCollection<TournamentComment> TournamentComments { get; set; }
        public ObservableCollection<Team> Teams { get; set; }
        private ObservableCollection<Team> teamsToAdd;
        public ObservableCollection<Team> TeamsToAdd
        {
            get => teamsToAdd;
            set
            {
                teamsToAdd = value; OnPropertyChanged();
            }
        }
        public ObservableCollection<Team> teamsToRemove;
        public ObservableCollection<Team> TeamsToRemove
        {
            get => teamsToRemove;
            set { teamsToRemove = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Match> Matches { get; set; }
        private RelayCommand? selectLogoCommand;
        public RelayCommand? SelectLogoCommand
        {
            get
            {
                return selectLogoCommand ??
                    (selectLogoCommand = new RelayCommand((obj) =>
                    {
                        try
                        {
                            var openedImage = ImageConverter.OpenAndLoadImage();
                            if (openedImage is byte[] imgByte)
                            {
                                TmpTournamentIcon = imgByte;
                            }
                        }
                        catch (Exception ex)
                        {
                            StatusService.Instance.SetStatusMessage(ex.Message);
                        }
                    }));
            }
        }
        private RelayCommand? dismissChangesCommand;
        public RelayCommand? DismissChangesCommand
        {
            get
            {
                return dismissChangesCommand ??
                    (dismissChangesCommand = new RelayCommand((obj) =>
                    {
                        TmpTournamentIcon = null;
                        NewTournamentName = null;
                        IsChanging = false;
                    }));
            }
        }
        private RelayCommand? removeTeamCommand;
        public RelayCommand? RemoveTeamCommand
        {
            get
            {
                return removeTeamCommand ??
                    (removeTeamCommand = new RelayCommand((obj) =>
                    {
                        try
                        {
                            if (TeamToRemove is Team && TeamToRemove != null)
                            {                                          
                                TeamsToAdd.Add(TeamToRemove);
                                StatusService.Instance.SetStatusMessage($"Team removed");
                                TeamsToRemove.Remove(TeamToRemove);                               
                            }
                            else
                            {
                                throw new Exception("Team not selected");
                            }

                        }
                        catch (Exception ex)
                        {
                            StatusService.Instance.SetStatusMessage(ex.Message);
                        }
                    }));
            }
        }
        private RelayCommand? addTeamCommand;
        public RelayCommand? AddTeamCommand
        {
            get
            {
                return addTeamCommand ??
                    (addTeamCommand = new RelayCommand((obj) =>
                    {
                        if (TeamToAdd is Team && TeamToAdd != null)
                        {
                            TeamsToRemove.Add(TeamToAdd);
                            StatusService.Instance.SetStatusMessage($"Team {TeamToAdd.TeamName} Add");
                            TeamsToAdd.Remove(TeamToAdd);
                        }
                        else
                        {
                            StatusService.Instance.SetStatusMessage("Team not selected");
                        }
                    }));
            }
        }
        private RelayCommand? updateProfileCommand;
        public RelayCommand? UpdateProfileCommand
        {
            get
            {
                return updateProfileCommand ??
                    (updateProfileCommand = new RelayCommand((obj) =>
                    {
                        try
                        {
                            Tournament tmp = ShowedTournament;
                            bool equal = !Teams.Select(t => t.TeamId).Except(TeamsToRemove.Select(t => t.TeamId)).Any()
                                        && !TeamsToRemove.Select(t => t.TeamId).Except(Teams.Select(t => t.TeamId)).Any();
                            
                            if (string.IsNullOrEmpty(NewTournamentName) && equal && TmpTournamentIcon == TournamentIcon)
                            {
                                throw new Exception("Nothing to update");
                            }
                            if (!string.IsNullOrEmpty(NewTournamentName))
                            {
                                tmp.TournamentName = NewTournamentName;
                            }
                            if (TournamentIcon != TmpTournamentIcon)
                            {
                                tmp.Img = TmpTournamentIcon;
                            }
                            if (!equal)
                            {
                                TournamentTeam tmpTT = new TournamentTeam();
                                if (Teams.Count() > 0)
                                {
                                    foreach (Team team in Teams)
                                    {
                                        tmpTT = uow.TournamentTeams.GetAll().Where(a => a.TeamId == team.TeamId && a.TournamentId == ShowedTournament.TournamentId).FirstOrDefault();
                                        uow.TournamentTeams.Delete(tmpTT);
                                    }
                                }
                                foreach (Team team in TeamsToRemove)
                                {
                                    tmpTT = new TournamentTeam();
                                    tmpTT.TournamentId = ShowedTournament.TournamentId;
                                    tmpTT.TeamId = team.TeamId;
                                    uow.TournamentTeams.Add(tmpTT);
                                }
                            }
                            uow.Tournaments.Update(tmp);
                            uow.Save();
                            ContentNavigationService.Instance.SwitchCurrentContentView(new TournamentPageView(uow.Tournaments.GetById(tmp.TournamentId)));
                        }
                        catch (Exception e)
                        {
                            StatusService.Instance.SetStatusMessage(e.Message);
                            uow.Dispose();
                        }
                    }));
            }
        }
        private TournamentComment? _selectedComment;
        public TournamentComment? SelectedComment
        {
            get => _selectedComment;
            set
            {
                if (_selectedComment != value)
                {
                    _selectedComment = value;
                    OnPropertyChanged();
                }
            }
        }
        public Team? TeamToRemove
        {
            get => teamToRemove;
            set { teamToRemove = value; OnPropertyChanged(); }
        }
        public Team? TeamToAdd
        {
            get => teamToAdd;
            set { teamToAdd = value; OnPropertyChanged(); }
        }
        private RelayCommand? changeTournamentCommand;
        public RelayCommand? ChangeTournamentCommand
        {
            get
            {
                return changeTournamentCommand ??= new RelayCommand((obj) =>
                {
                    IsChanging = false;
                    if (!IsChanging)
                    {
                        TmpTournamentIcon = TournamentIcon;
                        IsChanging = true;
                    }

                });
            }
        }
        public Tournament ShowedTournament
        {
            get => showedTournament;
            set { showedTournament = value; OnPropertyChanged(); }
        }
        public string TournamentName
        {
            get => tournamentName;
            set { tournamentName = value; OnPropertyChanged(); }
        }
        public string CommentContent
        {
            get => commentContent;
            set { commentContent = value; OnPropertyChanged(); }

        }
        private RelayCommand? nextPageCommand;
        public RelayCommand? NextPageCommand
        {
            get
            {
                return nextPageCommand ??= new RelayCommand((obj) =>
                {
                    if (CurrentCommentsPage < AmountOfCommentsPages)
                    {
                        CurrentCommentsPage++;
                        LoadComments(CurrentCommentsPage, 3); 
                    }
                });
            }
        }

        private RelayCommand? previousPageCommand;
        public RelayCommand? PreviousPageCommand
        {
            get
            {
                return previousPageCommand ??= new RelayCommand((obj) =>
                {
                    if (CurrentCommentsPage > 1)
                    {
                        CurrentCommentsPage--;
                        LoadComments(CurrentCommentsPage, 3); 
                    }
                });
            }
        }

        private RelayCommand? nextMatchesPageCommand;
        public RelayCommand? NextMatchesPageCommand
        {
            get
            {
                return nextMatchesPageCommand ??= new RelayCommand((obj) =>
                {
                    if (CurrentMatchesPage < AmountOfMatchesPages)
                    {
                        CurrentMatchesPage++;
                        LoadMatches(CurrentMatchesPage, 3);
                    }
                });
            }
        }

        private RelayCommand? previousMatchesPageCommand;
        public RelayCommand? PreviousMatchesPageCommand
        {
            get
            {
                return nextMatchesPageCommand ??= new RelayCommand((obj) =>
                {
                    if (CurrentMatchesPage > 1)
                    {
                        CurrentMatchesPage--;
                        LoadMatches(CurrentMatchesPage, 3);
                    }
                });
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
                            if (IsFavoriteTournament == true)
                            {
                                User user = CurrentUser;
                                user.FavTournamentId = null;
                                IsFavoriteTournament = false;
                                StatusService.Instance.SetStatusMessage("Unfavorite");
                                FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starEmpty.png");
                                uow.Users.Update(user);
                                uow.Save();
                                UserService.Instance.RenewCurrentUser(user);
                            }
                            else
                            {
                                User user = CurrentUser;
                                user.FavTournamentId = ShowedTournament.TournamentId;
                                IsFavoriteTournament = true;
                                StatusService.Instance.SetStatusMessage("Favorite");
                                FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starFill.png");
                                uow.Users.Update(user);
                                uow.Save();
                                UserService.Instance.RenewCurrentUser(user);
                            }
                        }
                        catch (Exception ex)
                        {
                            StatusService.Instance.SetStatusMessage(ex.ToString());
                        }


                    }));
            }
        }
        private RelayCommand? toTeamPageCommand;
        public RelayCommand? ToTeamPageCommand
        {
            get
            {
                return toTeamPageCommand ??
                    (toTeamPageCommand = new RelayCommand((obj) =>
                    {
                        if (obj is Team team)
                        {
                            ContentNavigationService.Instance.SwitchCurrentContentView(new TeamPageView(team));
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
        private RelayCommand? addMessageCommand;
        public RelayCommand? AddMessageCommand
        {
            get
            {
                return addMessageCommand ??
                    (addMessageCommand = new RelayCommand((obj) =>
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(CommentContent))
                            {
                                throw new Exception("Empty comment");
                            }
                            TournamentComment tempComment = new TournamentComment();
                            tempComment.Content = CommentContent;
                            tempComment.Author = CurrentUser.UserId;
                            tempComment.TournamentId = ShowedTournament.TournamentId;
                            tempComment.CreatedAt = DateTime.UtcNow;
                            uow.TournamentComments.Add(tempComment);
                            uow.Save();
                            ContentNavigationService.Instance.SwitchCurrentContentView(new TournamentPageView(uow.Tournaments.GetById(ShowedTournament.TournamentId)));
                        }
                        catch (Exception e)
                        {
                            StatusService.Instance.SetStatusMessage(e.Message);
                        }
                    }));
            }
        }
        private RelayCommand? deleteMessageCommand;
        public RelayCommand? DeleteMessageCommand
        {
            get
            {
                return deleteMessageCommand ??
                    (deleteMessageCommand = new RelayCommand((obj) =>
                    {
                        try
                        {
                            if (SelectedComment is TournamentComment comment)
                            {

                                uow.TournamentComments.Delete(comment);
                                uow.Save();
                                TournamentComments.Remove(comment);
                                LoadComments(CurrentCommentsPage, 3);
                                StatusService.Instance.SetStatusMessage($"Comment from {comment.User.Username} deleted");
                            }
                        }
                        catch (Exception e)
                        {
                            StatusService.Instance.SetStatusMessage(e.Message);
                        }
                    }));
            }
        }
        public bool IsFavoriteTournament
        {
            get => isFavoriteTournament;
            set { isFavoriteTournament = value; OnPropertyChanged(); }
        }
        public byte[] FavoriteIcon
        {
            get { return favoriteIcon; }
            set { favoriteIcon = value; OnPropertyChanged(); }
        }
        public byte[] TournamentIcon
        {
            get { return tournamentIcon; }
            set { tournamentIcon = value; OnPropertyChanged(); }
        }
        public byte[] TmpTournamentIcon
        {
            get { return tmpTournamentIcon; }
            set { tmpTournamentIcon = value; OnPropertyChanged(); }
        }
        public TournamentPageVM(Tournament tournament)
        {
            uow = new UnitOfWork(new ApplicationContext());
            ContentNavigationService.Instance.NavigationChanged += OnContentChanged;
            UserService.Instance.UserChanged += OnUserChanged;
            TournamentComments = new ObservableCollection<TournamentComment>();

            ShowedTournament = uow.Tournaments.GetById(tournament.TournamentId);
            TmpTournamentIcon = ShowedTournament.Img;
            TeamsToRemove = new ObservableCollection<Team>(uow.Teams.GetAll().Where(a => a.Tournaments.Any(b => b.TournamentId == ShowedTournament.TournamentId)));
            TeamsToAdd = new ObservableCollection<Team>(uow.Teams.GetAll().Where(a => !a.Tournaments.Any(b => b.TournamentId == ShowedTournament.TournamentId)));
            FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starEmpty.png");
            if (CurrentUser != null && CurrentUser.FavTournamentId == ShowedTournament.TournamentId)
            {
                IsFavoriteTournament = true;
                FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starFill.png");
            }
            TournamentName = ShowedTournament.TournamentName;
            TournamentIcon = ShowedTournament.Img;
            if (ShowedTournament.Teams.Count() > 0)
            {
                Teams =new ObservableCollection<Team>(ShowedTournament.Teams.Select(tt => tt.Team));
            }
            else
            {
                Teams = new ObservableCollection<Team>();
            }
            if (ShowedTournament.Matches.Count() > 0)
            {
                Matches = new ObservableCollection<Match>(ShowedTournament.Matches);
                LoadMatches(CurrentMatchesPage, 4);
            }
            else
            {
                Matches = new ObservableCollection<Match>();
            }
            if (uow.TournamentComments.GetAll().Where(a => a.TournamentId == ShowedTournament.TournamentId).Count() > 0)
            {
                TournamentComments = new ObservableCollection<TournamentComment>(uow.TournamentComments.GetAll().Where(a => a.TournamentId == ShowedTournament.TournamentId));
                LoadComments(CurrentCommentsPage, 3);
            }
        }
        private void LoadComments(int pageNumber, int pageSize)
        {
            try
            {
                var result = uow.TournamentComments.Get(pageNumber, pageSize, a => a.CommentId, a => a.TournamentId == ShowedTournament.TournamentId);

                TournamentComments.Clear();
                foreach (var comment in result.items)
                {
                    TournamentComments.Add(comment);
                }

                amountOfCommentsPages = result.TotalPages;
                CurrentCommentsPage = pageNumber;
            }
            catch (Exception ex)
            {
                StatusService.Instance.SetStatusMessage($"Error loading comments: {ex.Message}");
            }
        }
        private void LoadMatches(int pageNumber, int pageSize)
        {
            try
            {
                var result = uow.Matches.Get(pageNumber, pageSize, a => a.MatchId, a => a.TournamentId == ShowedTournament.TournamentId);

                Matches.Clear();
                foreach (var comment in result.items)
                {
                    Matches.Add(comment);
                }

                amountOfCommentsPages = result.TotalPages;
                CurrentCommentsPage = pageNumber;
            }
            catch (Exception ex)
            {
                StatusService.Instance.SetStatusMessage($"Error loading matches: {ex.Message}");
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
