using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DAL.Entities;
using IGDB.Models;
using PL.MVVM.View;

namespace PL.MVVM.ViewModel
{
    public class GameVM : INotifyPropertyChanged
    {
        private BL.IBL myBL;
        /// <summary>
        /// observable collection to handle the games data
        /// </summary>
        public ObservableCollection<Games> games { get; set; }
        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    OnPropertyChanged(nameof(SelectedIndex));
                    OnPropertyChanged(nameof(CurrentDescription));
                }
            }
        }
        /// <summary>
        /// delete game when pressing the button by sending to the Bl the game to delete 
        /// </summary>
        public async void DeleteGame()
        {
            if (SelectedIndex < 0 || SelectedIndex >= games.Count) return;

            await myBL.DeleteGame(games[SelectedIndex]);
            MessageBox.Show("Game removed successfully from the Catalogue", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            ImageCollection.RemoveAt(SelectedIndex);
            games.RemoveAt(SelectedIndex);

            if (SelectedIndex >= games.Count)
            {
                SelectedIndex = games.Count - 1;
            }
        }

        private ObservableCollection<CarouselModel> _imageCollection = new ObservableCollection<CarouselModel>();

        public ObservableCollection<CarouselModel> ImageCollection
        {
            get { return _imageCollection; }
            set { _imageCollection = value; }
        }
        /// <summary>
        /// Current description to display the description of the selected game in the carousel binded in the View
        /// </summary>
        public string CurrentDescription
        {
            get
            {
                if (SelectedIndex >= 0 && SelectedIndex < ImageCollection.Count)
                {
                    return ImageCollection[SelectedIndex].Description;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// contructor
        /// </summary>
        public GameVM()
        {
            myBL = new BL.BL();
            games = new ObservableCollection<Games>(myBL.GetGames());
           
            foreach(var game in games)
            {
                ImageCollection.Add(new CarouselModel(game));
            }


        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    /// <summary>
    /// Carousel class to handle the carousel of syncfusion
    /// </summary>
    public class CarouselModel
    {
        public CarouselModel(Games game)
        {
            Image = game.CoverUrl;
            Description = game.Summary+ (game.ReleaseDate != new DateTime(1753, 1, 1)? "\nRelease date: " + game.ReleaseDate.ToString():"");
            
        }
        public string Image { get; set; }

        public string Description { get; set; }

    }

}

