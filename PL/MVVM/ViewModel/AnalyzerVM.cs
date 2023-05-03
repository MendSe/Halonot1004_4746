using DAL.Entities;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PL.MVVM.ViewModel
{
    public class AnalyzerVM : INotifyPropertyChanged
    {
        private BL.IBL myBL;
        List<PlayersTime> playersTimes;
        private DateTime _startDate;
        private DateTime _endDate;
        private DateTime _predictDate;
        private int _predictNum;
        public int PredictNum
        {
            get => _predictNum;
            set
            {
                _predictNum = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }
        public DateTime PredictDate
        {
            get => _predictDate;
            set
            {
                _predictDate = value;
                OnPropertyChanged();
            }
        }

        private Visibility _errorMessageVisibility;
        public Visibility ErrorMessageVisibility
        {
            get => _errorMessageVisibility;
            set
            {
                _errorMessageVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _errorMessagePredVisibility;
        public Visibility ErrorMessagePredVisibility
        {
            get => _errorMessagePredVisibility;
            set
            {
                _errorMessagePredVisibility = value;
                OnPropertyChanged();
            }
        }
        private Servers _currentServer;
        public Servers CurrentServer
        {
            get { return _currentServer; }
            set
            {
                if (_currentServer != value)
                {
                    _currentServer = value;
                    OnPropertyChanged(nameof(CurrentServer));
                }
            }
        }
        public List<string> Members { get; set; }
        private string _selectedMember;
        public string SelectedMember
        {
            get => _selectedMember;
            set
            {
                _selectedMember = value;
                OnPropertyChanged();
            }
        }
        public ICommand UpdateGraphCommand { get; }
        public ICommand PredictCommand { get; }

        private SeriesCollection _chartSeries;
        public SeriesCollection ChartSeries
        {
            get => _chartSeries;
            set
            {
                _chartSeries = value;
                OnPropertyChanged(nameof(ChartSeries));
            }
        }
        public AnalyzerVM()
        {
            myBL = new BL.BL();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            PredictDate = DateTime.Now;
            Members = new List<string> { "Counter Strike Global offensive", "Elden Ring", "PUBG", "Hogwarts Legacy", "The Binding of Isaac" };
            SelectedMember = Members.First();
            UpdateGraphCommand = new RelayCommand(_ => FillGraph());
            PredictCommand = new RelayCommand(_ => Predict());
            ChartSeries = new SeriesCollection
            {
                new ColumnSeries { Title = "Low", Values = new ChartValues<double>() },
                new ColumnSeries { Title = "Medium", Values = new ChartValues<double>() },
                new ColumnSeries { Title = "High", Values = new ChartValues<double>() }
            };
            ErrorMessageVisibility = Visibility.Hidden;
            ErrorMessagePredVisibility = Visibility.Hidden;
        }
        private async void FillGraph()
        {
            if ((StartDate.Year >= EndDate.Year && StartDate.Month >= EndDate.Month && StartDate.Day >= EndDate.Day) || EndDate > DateTime.Now)
            {
                ErrorMessageVisibility = Visibility.Visible;
                return;
            }
            Mouse.OverrideCursor = Cursors.Wait;
            ErrorMessageVisibility = Visibility.Hidden;
            CurrentServer = await myBL.RetrieveServerFromApiAsync(SelectedMember);
            double below, above,low=0, medium=0, high=0,sum=0; 
            below=CurrentServer.PlayersCount - (CurrentServer.PlayersCount*0.22);
            above = CurrentServer.PlayersCount + (CurrentServer.PlayersCount * 0.22);
            playersTimes = await myBL.RetrieveNumberOfPlayersTime(CurrentServer.PlayersCount, StartDate, EndDate);
            foreach (var playertime in playersTimes) {
                if (playertime.Num < below)
                {
                    low++;
                }
                else if (playertime.Num > above) {
                    high++;
                }
                else
                {
                    medium++;
                }
            }
            sum=low+medium+high;
            low = Math.Round((low / sum) * 100,2);
            medium = Math.Round((medium/sum) * 100,2);
            high = Math.Round((high / sum) * 100,2);


            
            // Call your BL function to get the values

            // Assuming you have already set up the ChartSeries in your ViewModel
            ChartSeries[0].Values.Clear();
            ChartSeries[0].Values.Add(low);

            ChartSeries[1].Values.Clear();
            ChartSeries[1].Values.Add(medium);

            ChartSeries[2].Values.Clear();
            ChartSeries[2].Values.Add(high);
            Mouse.OverrideCursor = null;
        }
        /// <summary>
        /// Function Predict triggered by button Predict to predict the number of players at a time given
        /// </summary>
        private async void Predict()
        {
            if (PredictDate.Year < DateTime.Now.Year)
            {
                ErrorMessagePredVisibility = Visibility.Visible;
                return;
            }
            else if (PredictDate.Year == DateTime.Now.Year && PredictDate.Month < DateTime.Now.Month)
            {
                ErrorMessagePredVisibility = Visibility.Visible;
                return;
            }
            else if (PredictDate.Year == DateTime.Now.Year && PredictDate.Month == DateTime.Now.Month && PredictDate.Day <= DateTime.Now.Day)
            {
                ErrorMessagePredVisibility = Visibility.Visible;
                return;
            }
            ErrorMessagePredVisibility = Visibility.Hidden;
            Mouse.OverrideCursor = Cursors.Wait;
            CurrentServer = await myBL.RetrieveServerFromApiAsync(SelectedMember);
            PredictNum= (await myBL.RetrieveNumberOfPlayersTime(CurrentServer.PlayersCount, PredictDate, PredictDate))[0].Num;
            Mouse.OverrideCursor = null;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}