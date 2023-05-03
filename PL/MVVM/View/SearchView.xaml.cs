using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Win32;
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
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class SearchView : UserControl
    {
        SearchVM viewM;
        public SearchView()
        {
            InitializeComponent();
            viewM = new SearchVM();
            this.DataContext = viewM;
        }
        private async void GameTile_PlusButtonClicked(object sender, string gameName)
        {
            viewM.SaveGame(gameName);
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            viewM.Search(SearchBox.Text);
        }
        private void GameTile_GameDescriptionMouseEnter(object sender, string gameDescription)
        {
            DescriptionTextBlock.Text = gameDescription;
        }

        private void GameTile_GameDescriptionMouseLeave(object sender, string gameDescription)
        {
            DescriptionTextBlock.Text = string.Empty;
        }

        private void Game3_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
