using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PL.MVVM.View
{   /// <summary>
    /// Game Tile is a usercontrol used in Search to create a template of an image + a button with 
    /// functionnalities like hovering on the image to make the button appear
    /// </summary>
    public partial class GameTile : UserControl
    {
        /// <summary>
        /// Property to delegate the image path
        /// </summary>
        public static readonly DependencyProperty GameImagePathProperty = DependencyProperty.Register(
            "GameImagePath",
            typeof(string),
            typeof(GameTile),
            new PropertyMetadata(string.Empty));
  
        public string GameImagePath
        {
            get => (string)GetValue(GameImagePathProperty);
            set => SetValue(GameImagePathProperty, value);
        }

        public static readonly DependencyProperty GameNameProperty = DependencyProperty.Register(
            "GameName",
            typeof(string),
            typeof(GameTile),
            new PropertyMetadata(string.Empty));
        /// <summary>
        /// Property to delegate the description
        /// </summary>
        public string GameDescription
        {
            get => (string)GetValue(GameDescriptionProperty);
            set => SetValue(GameDescriptionProperty, value);
        }

        public static readonly DependencyProperty GameDescriptionProperty = DependencyProperty.Register(
            "GameDescription",
            typeof(string),
            typeof(GameTile),
            new PropertyMetadata(string.Empty));
        /// <summary>
        /// Property to delegate the game name
        /// </summary>
        public string GameName
        {
            get => (string)GetValue(GameNameProperty);
            set => SetValue(GameNameProperty, value);
        }
        /// <summary>
        /// constructor
        /// </summary>
        public GameTile()
        {
            InitializeComponent();

            PlusButton.Click += (sender, e) => PlusButtonClicked?.Invoke(this, GameName);
            PlusButton.MouseEnter += (sender, e) => PlusButton.Visibility = Visibility.Visible;
            PlusButton.MouseLeave += (sender, e) => Plusbutton_MouseLeave(sender, e);


            DependencyPropertyDescriptor.FromProperty(GameImagePathProperty, typeof(GameTile)).AddValueChanged(this, OnGameImagePathChanged);
        }
        /// <summary>
        /// event handlers for the different functionnalities
        /// </summary>
        public event EventHandler<string> PlusButtonClicked;
        public event EventHandler<string> GameDescriptionMouseEnter;
        public event EventHandler<string> GameDescriptionMouseLeave;
        /// <summary>
        /// function to event for hovering on the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameImage_MouseEnter(object sender, MouseEventArgs e)
        {
            PlusButton.Visibility = Visibility.Visible;
            GameDescriptionMouseEnter?.Invoke(this, GameDescription);
        }
        /// <summary>
        /// function to handle when leaving the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameImage_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsMouseOverPlusButton)
            {
                PlusButton.Visibility = Visibility.Collapsed;
            }
            GameDescriptionMouseLeave?.Invoke(this, GameDescription);
        }


        private bool IsMouseOverPlusButton => PlusButton.IsMouseOver;
        /// <summary>
        /// function when the mouse leave the button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Plusbutton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsMouseOverPlusButton)
            {
                PlusButton.Visibility = Visibility.Collapsed;
            }
        }

        private void OnGameImagePathChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(GameImagePath))
            {
                GameImage.Source = null;
                GameImage.Source = new BitmapImage(new Uri(GameImagePath, UriKind.RelativeOrAbsolute));
            }
            else
            {
                GameImage.Source = null; 
            }
        }
    }
}
