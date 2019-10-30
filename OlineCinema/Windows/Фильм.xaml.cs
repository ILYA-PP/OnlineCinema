using System;
using System.Collections.Generic;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace OlineCinema
{
    public partial class Фильм : Window
    {
        private Uri mediaUri;
        private DB dB = new DB(Properties.Settings.Default.CinemaDBConnectionString);
        BitmapImage poster;
        private int contentID;
        private bool access = false;
        private double rentPrice, buyPrice;

        private void PlayB_Click(object sender, RoutedEventArgs e)
        {
            //запуск фильма, если оформлена подписка или он куплен или взят в аренду
            if (!access)
            {
                if ((bool)dB.ExecuteScalarQuery($"SELECT VIP FROM Пользователь WHERE Логин = '{CurrentUser.Login}'"))
                {
                    access = true;
                }
                else
                {
                    object result = dB.ExecuteScalarQuery($"SELECT Покупка FROM Владение WHERE КодКонтента = {contentID} AND Логин = '{CurrentUser.Login}'");
                    if (result != null)
                    {
                        if (!(bool)result)
                            dB.ExecuteQuery($"DELETE FROM Владение WHERE КодКонтента = {contentID} AND Логин = '{CurrentUser.Login}'");
                        access = true;
                    }
                    else
                    {
                        MessageBox.Show("Для просмотра требуется купить или арендовать контент!");
                        access = false;
                    }
                }
            }
            if (access)
            {
                mediaF.Play();
                minB.IsEnabled = maxB.IsEnabled = pauseB.IsEnabled = stopB.IsEnabled = true;
            }
        }
        private void PauseB_Click(object sender, RoutedEventArgs e)
        {
            //поставить фильм на паузу
            mediaF.Pause();
        }

        private void StopB_Click(object sender, RoutedEventArgs e)
        {
            //остановить фильм
            mediaF.Stop();
        }

        private void MaxB_Click(object sender, RoutedEventArgs e)
        {
            //развернуть плеер на весь экран
            mediaF.Pause();
            Плеер player = new Плеер(mediaUri, mediaF.Position);
            player.Owner = this;
            player.ShowDialog();
        }

        public Фильм(int ID)
        {
            InitializeComponent();
            //добавление данных о фильме на форму
            loginL.Content = CurrentUser.Login;
            mediaF.Play();
            mediaF.Pause();
            contentID = ID;
            string geners, countrys, directors, acthors;
            geners = countrys = directors = acthors = "";
            ControlButton cb = new ControlButton();
            cb.CreateElements(this, mainG);
            AddFromDB(ref geners, $"SELECT Наименование FROM Жанр WHERE КодЖанра = ", $"SELECT КодЖанра FROM ЖанрКонтента WHERE КодКонтента = {ID}", dB.GetConnection(), true);
            AddFromDB(ref countrys, $"SELECT Наименование FROM Страна WHERE КодСтраны = ", $"SELECT КодСтраны FROM СтраныКонтента WHERE КодКонтента = {ID}", dB.GetConnection(), true);
            AddFromDB(ref directors, $"SELECT Имя, Фамилия FROM Режиссёр WHERE КодРежиссёра = ", $"SELECT КодРежиссёра FROM РежиссёрыКонтента WHERE КодКонтента = {ID}", dB.GetConnection(), false);
            AddFromDB(ref acthors, $"SELECT Имя, Фамилия FROM Актёр WHERE КодАктёра = ", $"SELECT КодАктёра FROM АктёрыКонтента WHERE КодКонтента = {ID}", dB.GetConnection(), false);
            ContentTable ct = new ContentTable();
            dB.ExecuteReaderQuery($"SELECT * FROM Контент WHERE КодКонтента = {ID}", ct);
            for(int i = 0; i < ct.ID.Count; i++)
            {
                rentPrice = Math.Round(ct.RentalPrice[i], 2);
                buyPrice = Math.Round(ct.PurchasePrice[i], 2);
                poster = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + $@"\Images\{ct.Poster[i]}", UriKind.Absolute));
                posterI.Source = poster;
                nameL.Content = ct.ContentName[i];
                specificationL.Text += $"Рейтинг: {ct.Rating[i]}\nГод: {ct.Year[i]}\nПродолжительность: {ct.Duration[i]} мин.\nЖанр: {geners}\n" +
                    $"Страна: {countrys}\nРежиссёр: {directors}\nАктёры: {acthors}\nСюжет: {ct.Story[i]}\nЦена покупки: {buyPrice} руб.\n" +
                    $"Цена проката: {rentPrice} руб.\n";
            }
            List<string> strID = new List<string>();
            FileTable ft = new FileTable();
            ContentFile cf = new ContentFile();
            dB.ExecuteReaderQuery($"SELECT КодФайла FROM ФайлыКонтента WHERE КодКонтента = {ID}", cf);
            for(int i = 0; i < cf.DataID.Count;i++)
            {
                dB.ExecuteReaderQuery($"SELECT Ссылка FROM Файл WHERE КодФайла = {cf.DataID[i]}", ft);
                for (int j = 0; j < ft.Reference.Count; j++)
                {
                    seriesCB.Items.Add(ft.Reference[j].ToString());
                }
            }
            if(seriesCB.Items.Count > 0)
            {
                seriesCB.Text = seriesCB.Items[0].ToString();
                mediaUri = new Uri(AppDomain.CurrentDomain.BaseDirectory + $@"\Films\{seriesCB.Items[0]}", UriKind.Absolute);
                mediaF.Source = mediaUri;
            }
            CommentTable commentT = new CommentTable();
            dB.ExecuteReaderQuery($"SELECT * FROM Отзыв WHERE КодКонтента = {ID}", commentT);
            for (int i = 0; i < commentT.ID.Count; i++)
            {
                CommentBlock.GetBlock(commentSP, commentT.Text[i], commentT.Login[i]);
            }
        }

        private void AddFromDB(ref string s, string q1, string q2, SqlConnection con, bool solo)
        {
            List<string> resID = new List<string>();
            SqlCommand c = new SqlCommand(q2, con);
            SqlDataReader r = c.ExecuteReader();
            while (r.Read())
            {
                resID.Add(r[0].ToString());
            }
            r.Close();
            foreach(string str in resID)
            {
                c.CommandText = q1 + str;
                r = c.ExecuteReader();
                while (r.Read())
                {
                    if (solo)
                        s += r[0].ToString() + ", ";
                    else
                        s += $"{r[0].ToString()} {r[1].ToString()}" + ", ";
                }
                r.Close();
            }
            if (s.Length > 0)
                s = s.Remove(s.Length-2, 2);
        }

        private void SendB_Click(object sender, RoutedEventArgs e)
        {
            //добавление отзыва о фильме
            CommentBlock.GetBlock(commentSP, commentTB.Text, CurrentUser.Login);
            dB.ExecuteQuery($"INSERT INTO Отзыв VALUES({contentID},N'{CurrentUser.Login}',N'{commentTB.Text}')");
            commentTB.Clear();
        }

        private void RentB_BuyB_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Opacity = 100;
        }
        private void LoginL_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //Открытие формы Личный кабинет
            ЛичныйКабинет lc = new ЛичныйКабинет();
            lc.Show();
            Hide();
        }
        private void SeriesCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //остановка фильма
            mediaF.Stop();
            mediaUri = new Uri(AppDomain.CurrentDomain.BaseDirectory + $@"\Films\{seriesCB.SelectedItem}", UriKind.Absolute);
            mediaF.Source = mediaUri;
        }
        private void RentB_BuyB_Click(object sender, RoutedEventArgs e)
        {
            //открытие формы Оплата
            Оплата op;
            if (((Button)sender).Equals(buyB))
                op  = new Оплата(poster, nameL.Content.ToString(), buyPrice, "Покупка", contentID);
            else
                op = new Оплата(poster, nameL.Content.ToString(), rentPrice, "Прокат", contentID);
            op.Show();
        }
    }
}
