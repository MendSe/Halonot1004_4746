using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using PL.MVVM.View;

namespace PL.MVVM.ViewModel
{
    public class MenuVM : INotifyPropertyChanged
    {
        private UserControl _currentView;

        public UserControl CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }

        /// <summary>
        /// commands to display the different pages in the View model in the control content and to switch from a page to another
        /// </summary>
        public ICommand ShowServerCommand { get; set; }
        public ICommand ShowAnalyzeCommand { get; set; }
        public ICommand ShowGamesCommand { get; set; }
        public ICommand ShowSearchCommand { get; set; }



        public MenuVM()
        {
            ShowServerCommand = new RelayCommand(o => ShowServer());
            ShowAnalyzeCommand = new RelayCommand(o => ShowAnalyze());
            ShowGamesCommand = new RelayCommand(o => ShowGames());
            ShowSearchCommand = new RelayCommand(o => ShowSearch());
        }
        
        private void ShowServer()
        {
            CurrentView = new ServerView();
        }
        
        private void ShowAnalyze()
        {
            CurrentView = new AnalyzerView();
        }

        private void ShowGames()
        {
            CurrentView = new GamesView();
        }
        private void ShowSearch()
        {
            CurrentView = new SearchView();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
