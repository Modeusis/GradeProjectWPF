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
    internal class MatchesVM : ViewModelBase
    {
        private UnitOfWork uow;
        public UserControl? CurrentContentView => ContentNavigationService.Instance.CurrentContentView;
        public User? CurrentUser => UserService.Instance.CurrentUser;
        public bool IsAdmin => UserService.Instance.Admin;
        Func<Match, bool> findFunc;
        Func<Match, object> orderFunc = a => a.MatchId;
        private int currentMatchesPage = 1;
        private int amountOfMatchesPages = 1;
        private bool isOrdered = false;
        public bool IsOrdered
        {
            get => isOrdered;
            set
            {
                isOrdered = value;
                OnPropertyChanged();
                CurrentMatchesPage = 1;
                LoadMatches(CurrentMatchesPage, 6);
            }
        }
        public int CurrentMatchesPage
        {
            get => currentMatchesPage;
            set { currentMatchesPage = value; OnPropertyChanged(); }
        }
        public int AmountOfMatchesPages
        {
            get => amountOfMatchesPages;
            set { amountOfMatchesPages = value; OnPropertyChanged(); }
        }
        private string currentText;
        public string CurrentText
        {
            get => currentText;
            set
            {
                currentText = value;
                OnPropertyChanged();
                findFunc = (txt =>
                            (txt.FirstTeam.TeamName.Contains(currentText, StringComparison.OrdinalIgnoreCase) ||
                             txt.SecondTeam.TeamName.Contains(currentText, StringComparison.OrdinalIgnoreCase)));
                CurrentMatchesPage = 1;
                LoadMatches(CurrentMatchesPage, 6);
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
                        orderFunc = a => a.MatchTime;
                        IsOrdered = true;
                    }
                    else
                    {
                        orderFunc = a => a.MatchId;
                        IsOrdered = false;
                    }
                });
            }
        }

        public ObservableCollection<Match> SortedMatches { get; set; }
        private Match? _selectedItem;
        public Match? SelectedItem
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

        private RelayCommand? toMatchPageCommand;
        public RelayCommand? ToMatchPageCommand
        {
            get
            {
                return toMatchPageCommand ??
                    (toMatchPageCommand = new RelayCommand((obj) =>
                    {
                        if (obj is Match Match) ContentNavigationService.Instance.SwitchCurrentContentView(new MatchPageView(Match));
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
                    if (CurrentMatchesPage < AmountOfMatchesPages)
                    {
                        CurrentMatchesPage++;
                        LoadMatches(CurrentMatchesPage, 6);
                    }
                });
            }
        }

        private RelayCommand? deleteMatchCommand;
        public RelayCommand? DeleteMatchCommand
        {
            get
            {
                return deleteMatchCommand ??= new RelayCommand((obj) =>
                {
                    if (obj is Match Match)
                    {
                        StatusService.Instance.SetStatusMessage($"Deleting Match: {Match.MatchId}");
                        uow.Matches.Delete(Match);
                        uow.Save();
                        UserService.Instance.RenewCurrentUser(uow.Users.GetById(CurrentUser.UserId));
                        LoadMatches(CurrentMatchesPage, 6);
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
                    if (CurrentMatchesPage > 1)
                    {
                        CurrentMatchesPage--;
                        LoadMatches(CurrentMatchesPage, 6);
                    }
                });
            }
        }
        public MatchesVM()
        {
            uow = new UnitOfWork(new ApplicationContext());
            ContentNavigationService.Instance.NavigationChanged += OnContentChanged;
            UserService.Instance.UserChanged += OnUserChanged;

            SortedMatches = new ObservableCollection<Match>();

            if (uow.Matches.GetAll().Count() > 0)
            {
                LoadMatches(CurrentMatchesPage, 6);
            }
        }
        private void LoadMatches(int pageNumber, int pageSize)
        {
            try
            {
                var result = uow.Matches.Get(pageNumber, pageSize, orderFunc, findFunc);

                SortedMatches.Clear();
                foreach (var comment in result.items)
                {
                    SortedMatches.Add(comment);
                }

                amountOfMatchesPages = result.TotalPages;
                CurrentMatchesPage = pageNumber;
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
