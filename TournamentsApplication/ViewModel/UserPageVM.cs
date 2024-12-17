using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TournamentsApplication.Model;
using TournamentsApplication.Utility;
using TournamentsApplication.View;

namespace TournamentsApplication.ViewModel
{
    internal class UserPageVM : ViewModelBase
    {
        private UnitOfWork uow;
        public User? CurrentUser => UserService.Instance.CurrentUser;
        public byte[]? CurrentUserLogo => UserService.Instance.CurrentUser.Logo;
        public byte[]? CurrentUserHeader => UserService.Instance.CurrentUser.HeaderImg;
        public bool IsAdmin => UserService.Instance.Admin;
        public UserControl? CurrentView => NavigationService.Instance.CurrentView;
        public string? StatusText => StatusService.Instance.StatusText;
        public UserControl? CurrentContentView => ContentNavigationService.Instance.CurrentContentView;
        private string username;
        private string? newLogin;
        private string? newUsername;
        private Team? favTeam;
        private Player? favPlayer;
        private Tournament? favTournament;
        private bool isFavTeamExists = false;
        private bool isFavPlayerExists = false;
        private bool isFavTournamentExists = false;
        private bool isChangingProfile = false;
        private byte[] userImage;
        private byte[]? teamIcon;
        private byte[]? playerIcon;
        private byte[]? tmpUserLogo;
        private byte[]? tmpUserHeader;
        private string? teamName;
        private string? tournamentName;
        private string? playerName;
        private string? playerPosition;
        private RelayCommand? editProfileCommand;
        private RelayCommand? deleteProfileCommand;
        private RelayCommand? updateProfileCommand;
        private RelayCommand? teamClickedCommand;
        private RelayCommand? tournamentClickedCommand;
        private RelayCommand? selectLogoCommand;
        private RelayCommand? selectHeaderCommand;
        private RelayCommand? dismissChangesCommand;
        private RelayCommand? itemClickCommand;
        
        public ObservableCollection<Player>? TeamPlayers { get; set; }
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
                                TmpUserLogo = imgByte;
                            }
                        }
                        catch (Exception ex) 
                        {
                            StatusService.Instance.SetStatusMessage(ex.Message);
                        }
                    }));
            }
        }
        public RelayCommand? SelectHeaderCommand
        {
            get
            {
                return selectHeaderCommand ??
                    (selectHeaderCommand = new RelayCommand((obj) =>
                    {
                        try
                        {
                            var openedImage = ImageConverter.OpenAndLoadImage();
                            if (openedImage is byte[] imgByte)
                            {
                                TmpUserHeader = imgByte;
                            }
                        }
                        catch (Exception ex)
                        {
                            StatusService.Instance.SetStatusMessage(ex.Message);
                        }
                    }));
            }
        }
        public RelayCommand? DismissChangesCommand
        {
            get
            {
                return dismissChangesCommand ??
                    (dismissChangesCommand = new RelayCommand((obj) =>
                    {
                        TmpUserLogo = null;
                        TmpUserHeader = null;
                        NewLogin = null;
                        NewUsername = null;
                        Password = "";
                        NewPassword = "";
                        ConfirmNewPassword = "";
                        IsChangingProfile = false;
                    }));
            }
        }
        public RelayCommand? ItemClickCommand
        {
            get
            {
                return itemClickCommand ??
                    (itemClickCommand = new RelayCommand((obj) =>
                    {
                        if (obj is Player plr)
                        {
                            ContentNavigationService.Instance.SwitchCurrentContentView(new PlayerPageView(plr)); ;
                        }
                        
                    }));
            }
        }
        public RelayCommand? EditProfileCommand
        {
            get
            {
                return editProfileCommand ??
                    (editProfileCommand = new RelayCommand((obj) =>
                    {
                        TmpUserLogo = CurrentUserLogo;
                        TmpUserHeader = CurrentUserHeader;
                        IsChangingProfile = true;
                    }));
            }
        }
        
        public RelayCommand? UpdateProfileCommand
        {
            get
            {
                return updateProfileCommand ??
                    (updateProfileCommand = new RelayCommand((obj) =>
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(NewPassword) && string.IsNullOrEmpty(ConfirmNewPassword) && string.IsNullOrEmpty(NewUsername) 
                                && string.IsNullOrEmpty(NewLogin) && TmpUserHeader == CurrentUserHeader && TmpUserLogo == CurrentUserLogo)
                            {
                                throw new Exception("There are nothing to save");
                            }
                            User? tmpUser = CurrentUser;
                            string? whatSavedMessage = " ";
                            if (NewPassword != null)
                            {
                                if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmNewPassword))
                                {
                                    throw new Exception("Fill in all passwords fields to change password");
                                }
                                if (!PasswordHasher.VerifyPassword(Password, CurrentUser.Password))
                                {
                                    throw new Exception("Old password incorrect");
                                }
                                if (NewPassword == Password)
                                {
                                    throw new Exception("Old password equal to a new one");
                                }
                                PasswordHasher.ValidatePassword(NewPassword, ConfirmNewPassword);
                                tmpUser.Password = PasswordHasher.HashPassword(NewPassword);
                                whatSavedMessage += "Password ";
                            }
                            if (!string.IsNullOrEmpty(NewLogin)) 
                            {
                                if (Login == NewLogin)
                                {
                                    throw new Exception("New login similar to yours");
                                }
                                if (NewLogin.Length < 4)
                                {
                                    throw new Exception("At least 4 characters for login");
                                }
                                tmpUser.Login = NewLogin;
                                whatSavedMessage += "Login ";
                            }
                            if (!string.IsNullOrEmpty(NewUsername))
                            {
                                if (NewUsername.Length < 4)
                                {
                                    throw new Exception("At least 4 characters for username");
                                }
                                tmpUser.Username = NewUsername;
                                whatSavedMessage += "Username ";
                            }
                            if (TmpUserHeader != CurrentUserHeader)
                            {
                                tmpUser.HeaderImg = TmpUserHeader;
                                whatSavedMessage += "Header ";
                            }
                            if (TmpUserLogo != CurrentUserLogo)
                            {
                                tmpUser.Logo = TmpUserLogo;
                                whatSavedMessage += "Logo ";
                            }
                            tmpUser.UpdatedAt = DateTime.UtcNow;
                            uow.Users.Update(tmpUser);
                            UserService.Instance.RenewCurrentUser(tmpUser);
                            ContentNavigationService.Instance.SwitchCurrentContentView(new UserPageView());
                            StatusService.Instance.SetStatusMessage($"Account has been changed: {whatSavedMessage}");
                            uow.Save();
                        }
                        catch (Exception e)
                        {
                            StatusService.Instance.SetStatusMessage(e.Message);
                            uow.Dispose();
                        }
                    }));
            }
        }
        public RelayCommand? TeamClickedCommand
        {
            get
            {
                return teamClickedCommand ??
                    (teamClickedCommand = new RelayCommand((obj) =>
                    {
                        if (obj is Team team) ContentNavigationService.Instance.SwitchCurrentContentView(new TeamPageView(team));

                    }));
            }
        }
        public RelayCommand? TournamentClickedCommand
        {
            get
            {
                return tournamentClickedCommand ??
                    (tournamentClickedCommand = new RelayCommand((obj) =>
                    {
                        if (obj is Tournament tournament) ContentNavigationService.Instance.SwitchCurrentContentView(new TournamentPageView(tournament));
                    }));
            }
        }
        public RelayCommand? DeleteProfileCommand
        {
            get
            {
                return deleteProfileCommand ??
                    (deleteProfileCommand = new RelayCommand((obj) =>
                    {
                        try
                        {
                            UserService.Instance.DeleteCurrentUser();
                            StatusService.Instance.SetStatusMessage($"Account has been deleted");
                            NavigationService.Instance.SwitchCurrentView(new LoginView());
                        }
                        catch (Exception ex)
                        {
                            StatusService.Instance.SetStatusMessage(ex.Message);
                        }
                    }));
            }
        }
        public bool IsFavTeamExists
        {
            get { return isFavTeamExists; }
            set { isFavTeamExists = value; OnPropertyChanged(nameof(IsFavTeamExists)); }
        }
        public bool IsFavTournamentExists
        {
            get { return isFavTournamentExists; }
            set { isFavTournamentExists = value; OnPropertyChanged(nameof(IsFavTournamentExists)); }
        }
        public bool IsFavPlayerExists
        {
            get { return isFavPlayerExists; }
            set { isFavPlayerExists = value; OnPropertyChanged(nameof(IsFavPlayerExists)); }
        }
        public bool IsChangingProfile
        {
            get { return isChangingProfile; }
            set { isChangingProfile = value; OnPropertyChanged(nameof(IsChangingProfile)); }
        }
        public string? NewUsername
        {
            get { return newUsername;}
            set { newUsername = value; OnPropertyChanged(nameof(NewUsername)); }
        }
        public string? NewLogin
        {
            get { return newLogin; }
            set { newLogin = value; OnPropertyChanged(nameof(NewLogin)); }
        }
        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged(nameof(Username)); }
        }
        private string login;
        public string Login
        {
            get { return login; }
            set { login = value; OnPropertyChanged(nameof(Login)); }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(nameof(Password)); }
        }
        private string newPassword;
        public string NewPassword
        {
            get { return newPassword; }
            set { newPassword = value; OnPropertyChanged(nameof(NewPassword)); }
        }
        private string confirmNewPassword;
        public string ConfirmNewPassword
        {
            get { return confirmNewPassword; }
            set { confirmNewPassword = value; OnPropertyChanged(nameof(ConfirmNewPassword)); }
        }
       
        public byte[]? TmpUserLogo
        {
            get { return tmpUserLogo; }
            set { tmpUserLogo = value; OnPropertyChanged(nameof(TmpUserLogo)); }
        }
        public byte[]? TmpUserHeader
        {
            get { return tmpUserHeader; }
            set { tmpUserHeader = value; OnPropertyChanged(nameof(TmpUserHeader)); }
        }
        public Team? FavTeam
        {
            get { return favTeam; }
            set { favTeam = value; OnPropertyChanged(nameof(FavTeam)); }
        }
        public Player? FavPlayer
        {
            get { return favPlayer; }
            set { favPlayer = value; OnPropertyChanged(nameof(FavPlayer)); }
        }
        public byte[]? PlayerIcon
        {
            get { return playerIcon; }
            set { playerIcon = value; OnPropertyChanged(nameof(PlayerIcon)); }
        }
        public string? PlayerName
        {
            get { return playerName; }
            set { playerName = value; OnPropertyChanged(nameof(PlayerName)); }
        }
        public string? PlayerPosition
        {
            get { return playerPosition; }
            set { playerPosition = value; OnPropertyChanged(nameof(PlayerPosition)); }
        }
        public Tournament? FavTournament
        {
            get { return favTournament; }
            set { favTournament = value; OnPropertyChanged(nameof(FavTournament)); }
        }
        public byte[]? TeamIcon
        {
            get { return teamIcon; }
            set { teamIcon = value; OnPropertyChanged(nameof(TeamIcon)); }
        }
        public string? TeamName
        {
            get { return teamName; }
            set { teamName = value; OnPropertyChanged(nameof(TeamName)); }
        }
        public string? TournamentName
        {
            get { return tournamentName; }
            set { tournamentName = value; OnPropertyChanged(nameof(TournamentName)); }
        }
        public UserPageVM()
        {
            uow = new UnitOfWork(new ApplicationContext());

            UserService.Instance.UserChanged += OnUserChanged;
            NavigationService.Instance.NavigationChanged += OnCurrentViewChanged;
            ContentNavigationService.Instance.NavigationChanged += OnCurrentContentChanged;
            StatusService.Instance.StatusChanged += OnStatusChanged;

            Username = CurrentUser.Username;
            Login = CurrentUser.Login;
            TmpUserLogo = CurrentUserLogo;
            TmpUserHeader = CurrentUserHeader;
            if (CurrentUser.FavTeamId != null)
            {
                Team team = uow.Teams.GetAll().Where(a => a.TeamId == CurrentUser.FavTeamId).FirstOrDefault();
                FavTeam = team;
                isFavTeamExists = true;
                TeamPlayers = new ObservableCollection<Player>(team.Players);
                TeamIcon = team.TeamLogo;
                TeamName = team.TeamName;
            }
            else
            {
                TeamName = "-";
            }
            if (CurrentUser.FavPlayerId != null)
            {
                Player player = uow.Players.GetAll().Where(a => a.PlayerId == CurrentUser.FavPlayerId).FirstOrDefault();
                FavPlayer = player;
                isFavPlayerExists = true;
                PlayerIcon = player.PlayerImg;
                PlayerName = player.PlayerName;
                PlayerPosition = player.Position;
            }
            if (CurrentUser.FavTournamentId != null)
            {
                Tournament tournament = uow.Tournaments.GetAll().Where(a => a.TournamentId == CurrentUser.FavTournamentId).FirstOrDefault();
                FavTournament = tournament;
                isFavTournamentExists = true;
                TournamentName = tournament.TournamentName;
            }
        }
        public void OnUserChanged()
        {
            OnPropertyChanged(nameof(CurrentUser));
            OnPropertyChanged(nameof(IsAdmin));
            OnPropertyChanged(nameof(CurrentUserLogo));
        }
        public void OnCurrentViewChanged()
        {
            OnPropertyChanged(nameof(CurrentView));
        }
        public void OnCurrentContentChanged()
        {
            OnPropertyChanged(nameof(CurrentContentView));
        }
        public void OnStatusChanged()
        {
            OnPropertyChanged(nameof(StatusText));
        }
    }
}
