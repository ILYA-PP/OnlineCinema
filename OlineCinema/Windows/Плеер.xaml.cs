using System;
using System.Windows;

namespace OlineCinema
{
    public partial class Плеер : Window
    {
        public Плеер(Uri uri, TimeSpan position)
        {
            InitializeComponent();
            mediaE.Source = uri;
            mediaE.Position = position;
            mediaE.Play();
        }
        private void PlayB_Click(object sender, RoutedEventArgs e)
        {
            //начать проигрывание фильма
            mediaE.Play();
        }
        private void PauseB_Click(object sender, RoutedEventArgs e)
        {
            //поставить фильм на паузу
            mediaE.Pause();
        }
        private void StopB_Click(object sender, RoutedEventArgs e)
        {
            //остановить фильм
            mediaE.Stop();
        }
        private void MinB_Click(object sender, RoutedEventArgs e)
        {
            //свернуть фильм
            Фильм film = this.Owner as Фильм;
            if(film != null)
            {
                film.mediaF.Position = mediaE.Position;
                Close();
            }      
        }
    }
}
