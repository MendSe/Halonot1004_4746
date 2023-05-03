using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PL.MVVM.View
{
    public partial class GameTile : UserControl
    {
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

        public string GameName
        {
            get => (string)GetValue(GameNameProperty);
            set => SetValue(GameNameProperty, value);
        }

        public GameTile()
        {
            InitializeComponent();

            PlusButton.Click += (sender, e) => PlusButtonClicked?.Invoke(this, GameName);
            PlusButton.MouseEnter += (sender, e) => PlusButton.Visibility = Visibility.Visible;
            PlusButton.MouseLeave += (sender, e) => Plusbutton_MouseLeave(sender, e);


            // Add this line to update the image when the GameImagePath property changes
            DependencyPropertyDescriptor.FromProperty(GameImagePathProperty, typeof(GameTile)).AddValueChanged(this, OnGameImagePathChanged);
        }

        public event EventHandler<string> PlusButtonClicked;
        public event EventHandler<string> GameDescriptionMouseEnter;
        public event EventHandler<string> GameDescriptionMouseLeave;

        private void GameImage_MouseEnter(object sender, MouseEventArgs e)
        {
            PlusButton.Visibility = Visibility.Visible;
            GameDescriptionMouseEnter?.Invoke(this, GameDescription);
        }

        private void GameImage_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsMouseOverPlusButton)
            {
                PlusButton.Visibility = Visibility.Collapsed;
            }
            GameDescriptionMouseLeave?.Invoke(this, GameDescription);
        }


        private bool IsMouseOverPlusButton => PlusButton.IsMouseOver;

        private void Plusbutton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsMouseOverPlusButton)
            {
                PlusButton.Visibility = Visibility.Collapsed;
            }
        }

        private bool IsMouseOverGameImage => GameImage.IsMouseOver;

        private void OnGameImagePathChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(GameImagePath))
            {
                GameImage.Source = new BitmapImage(new Uri(GameImagePath, UriKind.RelativeOrAbsolute));
            }
        }
    }
}
