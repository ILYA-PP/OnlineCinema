using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace OlineCinema
{
    public partial class Main : Window
    {
        private DB dB = new DB(Properties.Settings.Default.CinemaDBConnectionString);
        private BitmapImage bmi;
        public Main()
        {
            InitializeComponent();
            ControlButton cb = new ControlButton();
            cb.CreateElements(this, mainG);
            loginL.Content = CurrentUser.Login;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //заполнение фильтров Жанры иСтраны
            AddContent("", "");
            CountryTable ct = new CountryTable();
            GenerTable gt = new GenerTable();
            dB.ExecuteReaderQuery($"SELECT Наименование FROM Жанр", gt);
            for(int i = 0; i < gt.GenerName.Count; i++)
            { 
                generCB.Items.Add(new CheckBox());
                ((CheckBox)generCB.Items[generCB.Items.Count - 1]).Content = gt.GenerName[i].ToString();
            }
            dB.ExecuteReaderQuery($"SELECT Наименование FROM Страна", ct);
            for (int i = 0; i < ct.CountryName.Count; i++)
            {
                countryCB.Items.Add(new CheckBox());
                ((CheckBox)countryCB.Items[countryCB.Items.Count - 1]).Content = ct.CountryName[i].ToString();
            }
        }
        private void AddContent(string filtr, string attribute)
        {
            //применение фильтров к фильмам
            ContentTable ct = new ContentTable();
            wrapPanel.Children.Clear();
            switch (attribute)
            {
                case "": dB.ExecuteReaderQuery($"SELECT КодКонтента, Название, Постер FROM Контент", ct); break;
                case "name": dB.ExecuteReaderQuery($"SELECT КодКонтента, Название, Постер FROM Контент WHERE Название LIKE N'%{filtr}%'", ct); break;
                case "minRating": dB.ExecuteReaderQuery($"SELECT КодКонтента, Название, Постер FROM Контент WHERE Рейтинг >= {filtr}", ct); break;
                case "currentYear": dB.ExecuteReaderQuery($"SELECT КодКонтента, Название, Постер FROM Контент WHERE Год = {filtr}", ct); break;
                case "rating": dB.ExecuteReaderQuery($"SELECT КодКонтента, Название, Постер FROM Контент ORDER BY Рейтинг", ct); break;
                case "year": dB.ExecuteReaderQuery($"SELECT КодКонтента, Название, Постер FROM Контент ORDER BY Год", ct); break;
                case "gener": dB.ExecuteReaderQuery($"SELECT КодКонтента, Название, Постер FROM Контент WHERE КодКонтента = (SELECT КодКонтента FROM ЖанрКонтента WHERE КодЖанра = {filtr})", ct); break;
                case "country": dB.ExecuteReaderQuery($"SELECT КодКонтента, Название, Постер FROM Контент WHERE КодКонтента = (SELECT КодКонтента FROM СтраныКонтента WHERE КодСтраны = {filtr})", ct); break;
                default: MessageBox.Show("ERROR!"); break;
            }
            for(int i = 0; i < ct.ID.Count; i++)
            { 
                FilmBlock fb = new FilmBlock();
                bmi = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + $@"\Images\{ct.Poster[i]}", UriKind.Absolute));
                fb.CreateBlock(wrapPanel, wrapPanel.ActualWidth, bmi, ct.ContentName[i].ToString(), Int32.Parse(ct.ID[i].ToString()));
            }
        }
        private void SearchB_Click(object sender, RoutedEventArgs e)
        {
            //поиск фильмов по названию
            wrapPanel.Children.Clear();
            AddContent(searchTB.Text, "name");
        }
        private void RatingS_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //поиск фильмов по рейтингу
            wrapPanel.Children.Clear();
            ratingL.Content = String.Format("{0:N1}", ratingS.Value);
            AddContent($"{Math.Round(ratingS.Value, 2)}", "minRating");
        }
        private void LoginL_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //открытие окна Личный кабинет
            ЛичныйКабинет lc = new ЛичныйКабинет();
            lc.Show();
            Hide();
        }
        private void Filter_Checked(object sender, RoutedEventArgs e)
        {
            //фильтры новинки, по рейтингу, по году выпуска, по популярности
            if ((bool)noveltyRB.IsChecked)
                AddContent(DateTime.Now.Year.ToString(), "currentYear");
            else if ((bool)ratingRB.IsChecked)
                AddContent("", "rating");
            else if ((bool)releaseYearRB.IsChecked)
                AddContent("", "year");
            else if ((bool)popularityRB.IsChecked)
            {
                //???
            }
        }
        private void ViewGenerB_Click(object sender, RoutedEventArgs e)
        {
            //поиск фильма по жанру
            viewGenerB.Visibility = Visibility.Hidden;
            string filter = "";
            foreach (CheckBox check in generCB.Items)
                if ((bool)check.IsChecked)
                {
                    filter += dB.ExecuteScalarQuery($"SELECT КодЖанра FROM Жанр WHERE Наименование = N'{check.Content}'").ToString() + " AND КодЖанра = ";
                }
            if (filter == "")
                return;
            filter = filter.Remove(filter.Length-16, 16);
            AddContent(filter, "gener");
        }
        private void GenerCB_DropDownOpened(object sender, EventArgs e)
        {
            viewGenerB.Visibility = Visibility.Visible;
        }

        private void CountryCB_DropDownOpened(object sender, EventArgs e)
        {
            viewCountB.Visibility = Visibility.Visible;
        }

        private void ViewCountB_Click(object sender, RoutedEventArgs e)
        {
            //поиск фильмов по стране
            viewCountB.Visibility = Visibility.Hidden;
            string filter = "";
            foreach (CheckBox check in countryCB.Items)
                if ((bool)check.IsChecked)
                {
                    filter += dB.ExecuteScalarQuery($"SELECT КодСтраны FROM Страна WHERE Наименование = N'{check.Content}'").ToString() + " AND КодСтраны = ";
                }
            if (filter == "")
                return;
            filter = filter.Remove(filter.Length - 16, 16);
            AddContent(filter, "country");
        }
    }
}
