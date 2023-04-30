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

        public ICommand ShowServerCommand { get; set; }
        public ICommand ShowAnalyzeCommand { get; set; }
        public ICommand ShowGamesCommand { get; set; }

        public MenuVM()
        {
            //ShowServerCommand = new RelayCommand(o => ShowServer());
            //ShowAnalyzeCommand = new RelayCommand(o => ShowAnalyze());
            ShowGamesCommand = new RelayCommand(o => ShowGames());
        }
        /*
        private void ShowServer()
        {
            CurrentView = new ServerControl();
        }

        private void ShowAnalyze()
        {
            CurrentView = new AnalyzeControl();
        }*/

        private void ShowGames()
        {
            CurrentView = new GamesView();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
