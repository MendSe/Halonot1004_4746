using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace PL.MVVM.ViewModel
{
    public class ServerVM : INotifyPropertyChanged
    {
        private BL.IBL myBL;
        List<PlayersTime> playersTimes;
        List<PlayersTime> currentPlayersTimes;
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

        public ObservableCollection<string> ChartLabels { get; set; }

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

        private string _timeValue;
        public string TimeValue
        {
            get { return _timeValue; }
            set
            {
                if (_timeValue != value)
                {
                    _timeValue = value;
                    OnPropertyChanged(nameof(TimeValue));
                }
            }
        }
        public ServerVM()
        {
            // Initialize the ChartSeries property
            myBL = new BL.BL();
            ChartSeries = new SeriesCollection { new ColumnSeries { Title = "Your Data", Values = new ChartValues<double>() } };
            ChartLabels = new ObservableCollection<string>();
        }
        public async Task LoadData(string gameName = null, int numOfDays = 30, int division = 23)
        {
            CurrentServer = await myBL.RetrieveServerFromApiAsync(gameName);
            LoadChartData(CurrentServer.PlayersCount);
        }

        public async void LoadChartData(int numplayers,int numOfDays=30,int division=23)
        {
            // Retrieve the data for the given gameName
            
            if (numplayers != -1)
            {
                playersTimes = await myBL.RetrieveNumberOfPlayersTime(numplayers, DateTime.Now.AddDays(-30), DateTime.Now);
            }

            if (playersTimes == null) return;
            currentPlayersTimes = playersTimes.Skip(Math.Max(0, playersTimes.Count - ((24 * numOfDays)+1))).ToList();

            
            // Prepare the data for the chart
            var data = new ChartValues<double>();
            var labels = new List<string>();


            for (int i = 0; i < currentPlayersTimes.Count; i += division)
            {
                int aggregatedNum = 0;
                int count = 0;

                for (int j = 0; j < division && i + j < currentPlayersTimes.Count; j++)
                {
                    aggregatedNum += currentPlayersTimes[i + j].Num;
                    count++;
                }

                double averageNum = Math.Round((double)aggregatedNum / count);
                data.Add(averageNum);

                // Display the hour and minute in the label
                switch (numOfDays)
                {
                    case 2: 
                        labels.Add(currentPlayersTimes[i].Hour.ToString("dddd d HH:mm"));
                        break;
                    case 7:
                        labels.Add(currentPlayersTimes[i].Hour.ToString("dddd d HH:mm"));
                        break;
                    case 30:
                        labels.Add(currentPlayersTimes[i].Hour.ToString("dddd d MMM"));
                        break;
                    default:
                        break;
                }
            }

            // Clear and add new data to the existing ColumnSeries
            ChartSeries[0].Values.Clear();
            foreach (var value in data)
            {
                ChartSeries[0].Values.Add(value);
            }

            // Update the chart labels
            ChartLabels.Clear();
            foreach (var label in labels)
            {
                ChartLabels.Add(label);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
