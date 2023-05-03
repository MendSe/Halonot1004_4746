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
    /// Interaction logic for ServerTile.xaml
    /// User control controlled used in the ServerView and responsible for the images and Health and Analyze buttons
    /// with fonctionnalities like hovering on the image to make the button appear
    /// </summary>
    public partial class ServerTile : UserControl
    {
        /// <summary>
        /// Property to delegate the image path
        /// </summary>
        public static readonly DependencyProperty GameImagePathProperty = DependencyProperty.Register(
            "GameImagePath",
            typeof(string),
            typeof(ServerTile),
            new PropertyMetadata(string.Empty));

        public string GameImagePath
        {
            get => (string)GetValue(GameImagePathProperty);
            set => SetValue(GameImagePathProperty, value);
        }
        public static readonly DependencyProperty GameNameProperty = DependencyProperty.Register(
        "GameName",
        typeof(string),
        typeof(ServerTile),
        new PropertyMetadata(string.Empty));

        /// <summary>
        /// Property to delegate the game name for other functionnalities on the ServerView like display 
        /// the number of players on the server
        /// </summary>
        public string GameName
        {
            get => (string)GetValue(GameNameProperty);
            set => SetValue(GameNameProperty, value);
        }
        /// <summary>
        /// Load the Tile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerTile_Loaded(object sender, RoutedEventArgs e)
        {
            GameImage.Source = new BitmapImage(new Uri(GameImagePath, UriKind.RelativeOrAbsolute));
        }
        /// <summary>
        /// Event handlers for the buttons
        /// </summary>
        public event EventHandler<string> HealthButtonClicked;
        public event EventHandler<string> AnalyzeButtonClicked;
        /// <summary>
        /// Constructor
        /// </summary>
        public ServerTile()
        {
            InitializeComponent();

            HealthButton.Click += (sender, e) => HealthButtonClicked?.Invoke(this, GameName);
            AnalyzeButton.Click += (sender, e) => AnalyzeButtonClicked?.Invoke(this, GameName);

        }

        private void GameImage_MouseEnter(object sender, MouseEventArgs e)
        {
            HealthButton.Visibility = Visibility.Visible;
            AnalyzeButton.Visibility = Visibility.Visible;
        }

        private void GameImage_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsMouseOverHealthButton && !IsMouseOverAnalyzeButton)
            {
                HealthButton.Visibility = Visibility.Collapsed;
                AnalyzeButton.Visibility = Visibility.Collapsed;
            }
        }

        private bool IsMouseOverHealthButton => HealthButton.IsMouseOver;
        private bool IsMouseOverAnalyzeButton => AnalyzeButton.IsMouseOver;

        private void Healthbutton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsMouseOverGameImage && !IsMouseOverAnalyzeButton)
            {
                HealthButton.Visibility = Visibility.Collapsed;
                AnalyzeButton.Visibility = Visibility.Collapsed;
            }
        }

        private void AnalyzeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsMouseOverGameImage && !IsMouseOverHealthButton)
            {
                HealthButton.Visibility = Visibility.Collapsed;
                AnalyzeButton.Visibility = Visibility.Collapsed;
            }
        }

        private bool IsMouseOverGameImage => GameImage.IsMouseOver;
    }
}