﻿using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using IronPython.Runtime.Operations;

namespace PL.MVVM.ViewModel
{
    public class SearchVM : INotifyPropertyChanged
    {
        private BL.IBL myBL;
        List<Games> games;

        private ObservableCollection<Games> _gameList;
        public ObservableCollection<Games> GameList
        {
            get { return _gameList; }
            set
            {
                _gameList = value;
                OnPropertyChanged(nameof(GameList));
            }
        }
        public SearchVM()
        {
            // Initialize the ChartSeries property
            myBL = new BL.BL();
            GameList = new ObservableCollection<Games>();
        }
        public async void Search(string gameName)
        {
            games = await myBL.RetrieveGamesFromApiAsync(gameName);
            foreach (Games game in games) { game.CoverUrl.Replace("t_thumb","t_cover_big"); }
            GameList = new ObservableCollection<Games>(games);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
