using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DAL.Entities;


namespace PL.MVVM.ViewModel
{
    public class GameVM
    {
        private BL.IBL myBL;
        public ObservableCollection<Games> games { get; set; }
        public ObservableCollection<ImageSource> CarouselImages { get; set; }

        public GameVM()
        {
            myBL = new BL.BL();
            games = new ObservableCollection<Games>(myBL.GetGames());
            CarouselImages = new ObservableCollection<ImageSource>();
            LoadImagesAsync();
        }

        private async Task LoadImagesAsync()
        {
            foreach (var game in games)
            {
                if (!string.IsNullOrEmpty(game.CoverUrl))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(game.CoverUrl);
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    CarouselImages.Add(image);
                }
            }
        }
    }
}
