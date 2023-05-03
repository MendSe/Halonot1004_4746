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
using Mono.Unix.Native;
using PL.MVVM.ViewModel;
using Syncfusion.Windows.Shared;

namespace PL.MVVM.View
{
    /// <summary>
    /// Interaction logic for GamesView.xaml
    /// </summary>
    public partial class GamesView : UserControl
    {
        public GameVM viewM { get; set; }

        public GamesView()
        {
            InitializeComponent();
            viewM = new GameVM();
            this.DataContext = viewM;
            GameCatalogue.SetBinding(Carousel.SelectedIndexProperty, new Binding(nameof(GameVM.SelectedIndex))
            {
                Mode = BindingMode.TwoWay
            });

            //this.GameCatalogue.ItemsSource = viewM.CarouselImages;
            //this.GameCatalogue.ItemsSource = viewM.ImageCollection;
        }
        /// <summary>
        /// Minus button to erase a game from catalogue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            viewM.DeleteGame();
        }
    }
}
