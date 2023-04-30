using PL.MVVM.View;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _imageMeats = "C:\\Users\\Lior\\source\\repos\\MendSe\\Halonot1004_4746\\PL\\images\\meats.png";
        public string imageMeats
        {
            get { return _imageMeats; }
            set { _imageMeats = value; }
        }

        public MainWindow()
        {
            InitializeComponent();

            // Set the DataContext of the Window to itself
            DataContext = this;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (Username.Text == "admin" && Password.Password == "1234")
            {
                MenuWindow menu = new MenuWindow();
                menu.Show();
                this.Close();
            }
            else
            {
                Error.Visibility = Visibility.Visible;
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
   
}
