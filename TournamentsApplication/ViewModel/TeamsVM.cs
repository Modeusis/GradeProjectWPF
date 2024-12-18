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
    internal class TeamsVM : ViewModelBase
    {
        private UnitOfWork uow;
        public UserControl? CurrentContentView => ContentNavigationService.Instance.CurrentContentView;
        public User? CurrentUser => UserService.Instance.CurrentUser;
        public bool IsAdmin => UserService.Instance.Admin;
        Func<Team, bool> findFunc;
        Func<Team, object> orderFunc = a => a.TeamId;
        private int currentTeamsPage = 1;
        private int amountOfTeamsPages = 1;
        private bool isOrdered = false;
        public bool IsOrdered
        {
            get => isOrdered;
            set 
            { 
                isOrdered = value;
                OnPropertyChanged();
                CurrentTeamsPage = 1;
                LoadTeams(CurrentTeamsPage, 6);
            }
        }
        public int CurrentTeamsPage
        {
            get => currentTeamsPage;
            set { currentTeamsPage = value; OnPropertyChanged(); }
        }
        public int AmountOfTeamsPages
        {
            get => amountOfTeamsPages;
            set { amountOfTeamsPages = value; OnPropertyChanged(); }
        }
        private string currentText;
        public string CurrentText
        {
            get => currentText;
            set
            {
                currentText = value;
                OnPropertyChanged();
                findFunc = (txt => txt.TeamName.Contains(currentText, StringComparison.OrdinalIgnoreCase));
                CurrentTeamsPage = 1;
                LoadTeams(CurrentTeamsPage, 6);
            }
        }
        private RelayCommand? currentOrderCommand;
        public RelayCommand? CurrentOrderCommand
        {
            get
            {
                return currentOrderCommand ??= new RelayCommand((obj) =>
                {
                    if (!IsOrdered)
                    {
                        orderFunc = a => a.WorldRanking;
                        IsOrdered = true;
                    }
                    else
                    {
                        orderFunc = a => a.TeamId;
                        IsOrdered = false;
                    }
                });
            }
        }

        public ObservableCollection<Team> SortedTeams { get; set; }
        private Team? _selectedItem;
        public Team? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged();
                }
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
                        if (obj is Team Team) ContentNavigationService.Instance.SwitchCurrentContentView(new TeamPageView(Team));
                    }));
            }
        }
        private RelayCommand? nextPageCommand;
        public RelayCommand? NextPageCommand
        {
            get
            {
                return nextPageCommand ??= new RelayCommand((obj) =>
                {
                    if (CurrentTeamsPage < AmountOfTeamsPages)
                    {
                        CurrentTeamsPage++;
                        LoadTeams(CurrentTeamsPage, 6);
                    }
                });
            }
        }

        private RelayCommand? deleteTeamCommand;
        public RelayCommand? DeleteTeamCommand
        {
            get
            {
                return deleteTeamCommand ??= new RelayCommand((obj) =>
                {
                    if (obj is Team Team)
                    {
                        StatusService.Instance.SetStatusMessage($"Deleting Team: {Team.TeamName}");
                        uow.Teams.Delete(Team);
                        uow.Save();
                        UserService.Instance.RenewCurrentUser(uow.Users.GetById(CurrentUser.UserId));
                        LoadTeams(CurrentTeamsPage, 6);
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
                    if (CurrentTeamsPage > 1)
                    {
                        CurrentTeamsPage--;
                        LoadTeams(CurrentTeamsPage, 6);
                    }
                });
            }
        }
        public TeamsVM()
        {
            uow = new UnitOfWork(new ApplicationContext());
            ContentNavigationService.Instance.NavigationChanged += OnContentChanged;
            UserService.Instance.UserChanged += OnUserChanged;

            SortedTeams = new ObservableCollection<Team>();

            if (uow.Teams.GetAll().Count() > 0)
            {
                LoadTeams(CurrentTeamsPage, 6);
            }
        }
        private void LoadTeams(int pageNumber, int pageSize)
        {
            try
            {
                var result = uow.Teams.Get(pageNumber, pageSize, orderFunc, findFunc);

                SortedTeams.Clear();
                foreach (var comment in result.items)
                {
                    SortedTeams.Add(comment);
                }

                amountOfTeamsPages = result.TotalPages;
                CurrentTeamsPage = pageNumber;
            }
            catch (Exception ex)
            {
                StatusService.Instance.SetStatusMessage($"Error loading comments: {ex.Message}");
            }
        }
        private void OnContentChanged()
        {
            OnPropertyChanged(nameof(CurrentContentView));
        }
        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(CurrentUser));
            OnPropertyChanged(nameof(IsAdmin));
        }
    }
}
