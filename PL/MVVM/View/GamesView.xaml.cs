using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using PL.MVVM.ViewModel;

namespace PL.MVVM.View
{
    /// <summary>
    /// Interaction logic for Games.xaml
    /// </summary>
    public partial class Games : Window
    {
        public GameVM viewM { get; set; }
        public Games()
        {
            InitializeComponent();
            viewM = new GameVM();
            this.DataContext = viewM;
            this.GameCatalogue.ItemsSource = viewM.games.CoverUrl;
        }
    }
}
