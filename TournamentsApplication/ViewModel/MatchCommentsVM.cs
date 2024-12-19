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
using TournamentsApplication.VIew;

namespace TournamentsApplication.ViewModel
{
    internal class MatchCommentsVM : ViewModelBase
    {
        private UnitOfWork uow;
        public UserControl? CurrentContentView => ContentNavigationService.Instance.CurrentContentView;
        public User? CurrentUser => UserService.Instance.CurrentUser;
        public bool IsLogin => UserService.Instance.Login;
        public bool IsAdmin => UserService.Instance.Admin;
        private int amountOfCommentsPages = 1;
        private int currentCommentsPage = 1;
        private Match matchForComments;
        private string commentContent;
        public string CommentContent
        {
            get => commentContent;
            set { commentContent = value; OnPropertyChanged(); }

        }
        public Match MatchForComments
        {
            get => matchForComments;
            set
            {
                matchForComments = value;
                OnPropertyChanged();
            }
        }
        private MatchComment? _selectedComment;
        public MatchComment? SelectedComment
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
        private RelayCommand? switchStatistic;
        public RelayCommand SwitchStatistic
        {
            get
            {
                return switchStatistic ??= new RelayCommand((obj) =>
                {
                    try
                    {
                        ContentNavigationService.Instance.SwitchCurrentContentView(new MatchPageView(uow.Matches.GetById(MatchForComments.MatchId)));
                    }
                    catch (Exception e)
                    {
                        StatusService.Instance.SetStatusMessage(e.Message);
                    }
                });
            }
        }
        public int CurrentCommentsPage
        {
            get => currentCommentsPage;
            set { currentCommentsPage = value; OnPropertyChanged(); }
        }
        public int AmountOfCommentsPages
        {
            get => amountOfCommentsPages;
            set { amountOfCommentsPages = value; OnPropertyChanged(); }
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
                        LoadComments(CurrentCommentsPage, 5);
                    }
                });
            }
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
                        LoadComments(CurrentCommentsPage, 5);
                    }
                });
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
                            if (SelectedComment is MatchComment comment)
                            {

                                uow.MatchComments.Delete(comment);
                                uow.Save();
                                MatchComments.Remove(comment);
                                LoadComments(CurrentCommentsPage, 5);
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
                            MatchComment tempComment = new MatchComment();
                            tempComment.Content = CommentContent;
                            tempComment.Author = CurrentUser.UserId;
                            tempComment.MatchId = MatchForComments.MatchId;
                            tempComment.CreatedAt = DateTime.UtcNow;
                            uow.MatchComments.Add(tempComment);
                            uow.Save();
                            LoadComments(CurrentCommentsPage, 5);
                            ContentNavigationService.Instance.SwitchCurrentContentView(new MatchCommentsView(uow.Matches.GetById(MatchForComments.MatchId)));
                        }
                        catch (Exception e)
                        {
                            StatusService.Instance.SetStatusMessage(e.Message);
                        }
                    }));
            }
        }

        private ObservableCollection<MatchComment> matchComments;
        public ObservableCollection<MatchComment> MatchComments
        {
            get => matchComments;
            set { matchComments = value; OnPropertyChanged(); }
        }
        public MatchCommentsVM(Match match)
        {
            uow = new UnitOfWork(new ApplicationContext());
            MatchForComments = uow.Matches.GetById(match.MatchId);
            ContentNavigationService.Instance.NavigationChanged += OnContentChanged;
            UserService.Instance.UserChanged += OnUserChanged;

            MatchComments = new ObservableCollection<MatchComment>();

            
            LoadComments(CurrentCommentsPage, 5);
        }

        private void LoadComments(int pageNumber, int pageSize)
        {
            try
            {
                var result = uow.MatchComments.Get(pageNumber, pageSize, a => a.CreatedAt, a => a.MatchId == MatchForComments.MatchId);

                MatchComments.Clear();
                foreach (var comment in result.items)
                {
                    MatchComments.Add(comment);
                }

                amountOfCommentsPages = result.TotalPages;
                CurrentCommentsPage = pageNumber;
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
            OnPropertyChanged(nameof(IsLogin));
            OnPropertyChanged(nameof(IsAdmin));
        }
    }
}
