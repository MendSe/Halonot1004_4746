using Microsoft.AspNetCore.Mvc.ViewEngines;
using PL.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL.MVVM.View
{
    /// <summary>
    /// Interaction logic for ServerView.xaml
    /// </summary>
    public partial class ServerView : UserControl
    {
        public ServerVM viewM { get; set; }
        public ServerView()
        {
            InitializeComponent();
            viewM = new ServerVM();
            this.DataContext = viewM;
            viewM.TimeValue = "Month";
        }
        private void GameTile_HealthButtonClicked(object sender, string gameName)
        {
            ContentTabControl.SelectedIndex = 0;
            viewM.LoadData(gameName);
        }

        private void GameTile_AnalyzeButtonClicked(object sender, string gameName)
        {
            ContentTabControl.SelectedIndex = 1;
            viewM.LoadData(gameName);
        }
        private void TimeButtonClicked(object sender, RoutedEventArgs e)
        {
            // Get the name of the clicked button
            string buttonName = ((Button)sender).Name;

            // Use the name of the button to determine which action to take
            switch (buttonName)
            {
                case "two":
                    viewM.LoadChartData(-1, 2,1);
                    viewM.TimeValue = "2d";
                    break;
                case "week":
                    viewM.LoadChartData(-1, 7,3);
                    viewM.TimeValue = "Week";
                    break;
                case "month":
                    viewM.LoadChartData(-1, 30, 23); // Aggregate hourly data points into daily data points
                    viewM.TimeValue = "Month";
                    break;
            }
        }


    }
}
