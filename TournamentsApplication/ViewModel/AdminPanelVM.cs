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
    internal class AdminPanelVM : ViewModelBase
    {
        private UnitOfWork uow;
        private string newTournamentName;
        private string newPlayerName;
        private string newFullPlayerName;
        private string newPositionName;
        private string newTeamName;
        private string newBirthdayDate;
        private string newWorldRank;
        private byte[]? tmpTournamentIcon;
        private byte[]? newPlayerImg;
        private byte[]? newTeamImg;
        private Tournament selectedTournament;
        private Team selectedFirstTeam;
        private Team selectedSecondTeam;
        private Player selectedPlayer;
        private Discipline selectedDiscipline;

        public ObservableCollection<Tournament> Tournaments { get; set; }
        private ObservableCollection<Team> teams;
        public ObservableCollection<Team> Teams
        { 
            get { return teams; } 
            set { teams = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Player> Players { get; set; }
        public ObservableCollection<Discipline> Disciplines { get; set; }
        public ObservableCollection<Team> AvailableTeams { get; set; }
        public Tournament SelectedTournament
        {
            get { return selectedTournament; }
            set 
            { 
                selectedTournament = value;
                Teams = new ObservableCollection<Team>(uow.Teams.GetAll().Where(a => 
                a.Tournaments.Any(b => b.TournamentId == selectedTournament.TournamentId)).Where(a => a.Players.Count() == 5));
                OnPropertyChanged();
            }
        }
        public Team SelectedFirstTeam
        {
            get { return selectedFirstTeam; }
            set { selectedFirstTeam = value; OnPropertyChanged(); }
        }
        public Team SelectedSecondTeam
        {
            get { return selectedSecondTeam; }
            set { selectedSecondTeam = value; OnPropertyChanged(); }
        }
        public Player SelectedPlayer
        {
            get { return selectedPlayer; }
            set { selectedPlayer = value; OnPropertyChanged(); }
        }
        public Discipline SelectedDiscipline
        {
            get { return selectedDiscipline; }
            set { selectedDiscipline = value; OnPropertyChanged(); }
        }
        public string NewTournamentName
        {
            get { return newTournamentName; }
            set { newTournamentName = value; OnPropertyChanged(); }
        }
        public string NewWorldRank
        {
            get { return newWorldRank; }
            set { newWorldRank = value; OnPropertyChanged(); }
        }
        public string NewPlayerName
        {
            get { return newPlayerName; }
            set { newPlayerName = value; OnPropertyChanged(); }
        }
        public string NewFullPlayerName
        {
            get { return newFullPlayerName; }
            set { newFullPlayerName = value; OnPropertyChanged(); }
        }
        public string NewPositionName
        {
            get { return newPositionName; }
            set { newPositionName = value; OnPropertyChanged(); }
        }
        public string NewBirthdayDate
        {
            get { return newBirthdayDate; }
            set { newBirthdayDate = value; OnPropertyChanged(); }
        }
        public string NewTeamName
        {
            get { return newTeamName; }
            set { newTeamName = value; OnPropertyChanged(); }
        }
        public byte[]? TmpTournamentIcon
        {
            get { return tmpTournamentIcon; }
            set { tmpTournamentIcon = value; OnPropertyChanged(); }
        }
        public byte[]? NewPlayerImg
        {
            get { return newPlayerImg; }
            set { newPlayerImg = value; OnPropertyChanged(); }
        }
        public byte[]? NewTeamImg
        {
            get { return newTeamImg; }
            set { newTeamImg = value; OnPropertyChanged(); }
        }

        
        private RelayCommand? addTournamentCommand;
        public RelayCommand? AddTournamentCommand
        {
            get
            {
                return addTournamentCommand ??= new RelayCommand(obj =>
                {
                    if (!IsAddTournament)
                    {
                        IsAddTournament = true;
                        IsAddTeam = false;
                        IsAddPlayer = false;
                        IsAddMatch = false;
                    }
                    
                });
            }
        }

        private RelayCommand? addMatchCommand;
        public RelayCommand? AddMatchCommand
        {
            get
            {
                return addMatchCommand ??= new RelayCommand(obj =>
                {
                    if (!IsAddMatch)
                    {
                        IsAddTournament = false;
                        IsAddTeam = false;
                        IsAddPlayer = false;
                        IsAddMatch = true;
                    }
                });
            }
        }

        private RelayCommand? addPlayerCommand;
        public RelayCommand? AddPlayerCommand
        {
            get
            {
                return addPlayerCommand ??= new RelayCommand(obj =>
                {
                    if (!IsAddPlayer)
                    {
                        IsAddTournament = false;
                        IsAddTeam = false;
                        IsAddPlayer = true;
                        IsAddMatch = false;
                    }
                });
            }
        }

        private RelayCommand? addTeamCommand;
        public RelayCommand? AddTeamCommand
        {
            get
            {
                return addTeamCommand ??= new RelayCommand(obj =>
                {
                    if (!IsAddTeam)
                    {
                        IsAddTournament = false;
                        IsAddTeam = true;
                        IsAddPlayer = false;
                        IsAddMatch = false;
                    }
                });
            }
        }

        private RelayCommand? selectTournamentLogoCommand;
        public RelayCommand? SelectTournamentLogoCommand
        {
            get
            {
                return selectTournamentLogoCommand ??= new RelayCommand(obj =>
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
                });
            }
        }

        private RelayCommand? addTournamentConfirmCommand;
        public RelayCommand? AddTournamentConfirmCommand
        {
            get
            {
                return addTournamentConfirmCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        if (SelectedDiscipline == null || string.IsNullOrEmpty(NewTournamentName) || TmpTournamentIcon == null)
                        {
                            throw new ArgumentNullException("Fill all fields");
                        }
                        Tournament trnmnt = new Tournament();
                        trnmnt.DisciplineId = SelectedDiscipline.DisciplineId;
                        trnmnt.TournamentName = NewTournamentName;
                        trnmnt.Img = TmpTournamentIcon;
                        uow.Tournaments.Add(trnmnt);
                        uow.Save();
                        ContentNavigationService.Instance.SwitchCurrentContentView(new TournamentsView());
                        StatusService.Instance.SetStatusMessage("Created succesful");
                    }
                    catch (Exception e)
                    {
                        StatusService.Instance.SetStatusMessage(e.Message);
                    }
                });
            }
        }

        

        private RelayCommand? addMatchConfirmCommand;
        public RelayCommand? AddMatchConfirmCommand
        {
            get
            {
                return addMatchConfirmCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        IQueryable<Match> currentMatches = uow.Matches.GetAll().Where(a => a.Status == false);
                        if (SelectedTournament == null || SelectedFirstTeam == null || SelectedSecondTeam == null)
                        {
                            throw new Exception("Select all fields");
                        }
                        if (SelectedFirstTeam.TeamId == SelectedSecondTeam.TeamId)
                        {
                            throw new Exception("Select diferent teams fields");
                        }
                        foreach (Match match in currentMatches)
                        {
                            if (match.FirstParticipantId == SelectedFirstTeam.TeamId || match.SecondParticipantId == SelectedFirstTeam.TeamId)
                            {
                                throw new Exception("First team already in match");
                            }
                            if (match.FirstParticipantId == SelectedSecondTeam.TeamId || match.SecondParticipantId == SelectedSecondTeam.TeamId)
                            {
                                throw new Exception("Second team already in match");
                            }
                        }
                        Match mtch = new Match();
                        mtch.TournamentId = SelectedTournament.TournamentId;
                        mtch.FirstParticipantId = SelectedFirstTeam.TeamId;
                        mtch.SecondParticipantId = SelectedSecondTeam.TeamId;
                        mtch.MatchTime = DateTime.UtcNow;
                        uow.Matches.Add(mtch);
                        uow.Save();
                        ContentNavigationService.Instance.SwitchCurrentContentView(new MatchesView());
                        StatusService.Instance.SetStatusMessage("Created succesful");
                    }
                    catch (Exception e)
                    {
                        StatusService.Instance.SetStatusMessage(e.Message);
                    }
                });
            }
        }
        private RelayCommand? addPlayerConfirmCommand;
        public RelayCommand? AddPlayerConfirmCommand
        {
            get
            {
                return addPlayerConfirmCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        if (NewPlayerImg == null || string.IsNullOrEmpty(NewPlayerName) || 
                        string.IsNullOrEmpty(NewPlayerName) || string.IsNullOrEmpty(NewFullPlayerName) 
                        || string.IsNullOrEmpty(NewPositionName) || string.IsNullOrEmpty(NewBirthdayDate))
                        {
                            throw new ArgumentNullException("Select all fields");
                        }
                        DateTime tmpDate;
                        if (!DateTime.TryParse(NewBirthdayDate,out tmpDate))
                        {
                            throw new ArgumentNullException("Invalid birth date");
                        };
                        Player tmp = new Player();
                        tmp.PlayerName = NewPlayerName;
                        tmp.PlayerRealName = NewFullPlayerName;
                        tmp.Position = NewPositionName;
                        tmp.PlayerImg = NewPlayerImg;
                        if (SelectedTeam != null)
                        {
                            tmp.CurTeamId = SelectedTeam.TeamId;
                        }
                        tmp.CreatedAt = DateTime.UtcNow;
                        tmp.BirthDayDate = tmpDate.ToUniversalTime();
                        uow.Players.Add(tmp);
                        uow.Save();
                        ContentNavigationService.Instance.SwitchCurrentContentView(new PlayersView());
                        StatusService.Instance.SetStatusMessage("Created succesful");
                    }
                    catch (Exception e)
                    {
                        StatusService.Instance.SetStatusMessage(e.Message);
                    }
                });
            }
        }

        private RelayCommand? addTeamConfirmCommand;
        public RelayCommand? AddTeamConfirmCommand
        {
            get
            {
                return addTeamConfirmCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        Team tmpTeam = new Team();
                        if (NewTeamImg == null && string.IsNullOrEmpty(NewTeamName) && string.IsNullOrEmpty(NewWorldRank))
                        {
                            throw new Exception("Fill all fields");
                        }
                        int WorldRank;
                        if (!int.TryParse(NewWorldRank, out WorldRank))
                        {
                            throw new Exception("Incorrect world rank");
                        }
                        if (WorldRank < 0)
                        {
                            throw new Exception("Incorrect world rank");
                        }
                        int amountOfTeams = uow.Teams.GetAll().Count();
                        if (WorldRank > amountOfTeams + 1)
                        {
                            WorldRank = amountOfTeams + 1;
                        }
                        tmpTeam.WorldRanking = WorldRank;
                        tmpTeam.TeamName = NewTeamName;
                        tmpTeam.TeamLogo = NewTeamImg;
                        var TeamCollection = uow.Teams.GetAll().Where(a => a.WorldRanking >= WorldRank);
                        if (TeamCollection.Count() > 0)
                        {
                            foreach (var team in TeamCollection)
                            {
                                team.WorldRanking++;
                                uow.Teams.Update(team);
                            }
                        }
                        uow.Teams.Add(tmpTeam);
                        uow.Save();
                        ContentNavigationService.Instance.SwitchCurrentContentView(new TeamsView());
                        StatusService.Instance.SetStatusMessage("Created succesful");
                    }
                    catch (Exception e)
                    {
                        StatusService.Instance.SetStatusMessage(e.Message);
                    }
                });
            }
        }

        private bool isAddTournament = false;
        public bool IsAddTournament
        {
            get { return isAddTournament; }
            set
            {
                if (isAddTournament != value)
                {
                    isAddTournament = value;
                    OnPropertyChanged();  
                }
            }
        }

        private bool isAddPlayer = false;
        public bool IsAddPlayer
        {
            get { return isAddPlayer; }
            set
            {
                if (isAddPlayer != value)
                {
                    isAddPlayer = value;
                    OnPropertyChanged(); 
                }
            }
        }

        private bool isAddTeam = false;
        public bool IsAddTeam
        {
            get { return isAddTeam; }
            set
            {
                if (isAddTeam != value)
                {
                    isAddTeam = value;
                    OnPropertyChanged();  
                }
            }
        }

        private bool isAddMatch = false;
        public bool IsAddMatch
        {
            get { return isAddMatch; }
            set
            {
                if (isAddMatch != value)
                {
                    isAddMatch = value;
                    OnPropertyChanged();  
                }
            }
        }
        private ObservableCollection<Team> otherTeams;
        public ObservableCollection<Team> OtherTeams
        {
            get => otherTeams;
            set { otherTeams = value; OnPropertyChanged(); }
        }

        private Team selectedTournamentTeam;
        public Team SelectedTournamentTeam
        {
            get => selectedTournamentTeam;
            set { selectedTournamentTeam = value; OnPropertyChanged(); }
        }

        private Team selectedTeam;
        public Team SelectedTeam
        {
            get => selectedTeam;
            set { selectedTeam = value; OnPropertyChanged(); }
        }

        private RelayCommand selectPlayerLogoCommand;
        public RelayCommand SelectPlayerLogoCommand
        {
            get
            {
                return selectPlayerLogoCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        var openedImage = ImageConverter.OpenAndLoadImage();
                        if (openedImage is byte[] imgByte)
                        {
                            NewPlayerImg = imgByte;
                        }
                    }
                    catch (Exception ex)
                    {
                        StatusService.Instance.SetStatusMessage(ex.Message);
                    }
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


        public AdminPanelVM()
        {
            uow = new UnitOfWork(new ApplicationContext());
            Tournaments = new ObservableCollection<Tournament>();
            Teams = new ObservableCollection<Team>();
            Players = new ObservableCollection<Player>();
            Disciplines = new ObservableCollection<Discipline>();
            AvailableTeams = new ObservableCollection<Team>();

            Disciplines = new ObservableCollection<Discipline>(uow.Disciplines.GetAll());
            Tournaments = new ObservableCollection<Tournament>(uow.Tournaments.GetAll());
            AvailableTeams = new ObservableCollection<Team>(uow.Teams.GetAll().Where(a => a.Players.Count() < 5));
            Teams = new ObservableCollection<Team>();

            IsAddTournament = true;
        }
    }
}
