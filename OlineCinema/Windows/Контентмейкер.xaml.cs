using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OlineCinema
{
    public partial class Контентмейкер : Window
    {
        private static string table = "";
        private static string posterFile;
        private DB dB = new DB(Properties.Settings.Default.CinemaDBConnectionString);
        private CountryTable countryT = new CountryTable();
        private ContentTable contentT = new ContentTable();
        private ActhorTable acthorT = new ActhorTable();
        private DirectorTable directorT = new DirectorTable();
        private GenerTable generT = new GenerTable();
        BitmapImage bmi;
        public Контентмейкер()
        {
            InitializeComponent();
            //заполнение комбобоксов данными из БД
            dB.ExecuteReaderQuery($"SELECT Наименование FROM Страна", countryT);
            AddComboBoxContent(countryCB, countryT.CountryName, null, null);
            dB.ExecuteReaderQuery($"SELECT КодКонтента, Название FROM Контент ORDER BY Название", contentT);
            for (int i = 0; i < contentT.ID.Count; i++)
            {
                filmCB.Items.Add($"{contentT.ContentName[i]}-№{contentT.ID[i]}");
            }
            dB.ExecuteReaderQuery($"SELECT * FROM Актёр", acthorT);
            AddComboBoxContent(acthorCB, acthorT.LastName, acthorT.FirstName, acthorT.MiddleName);
            dB.ExecuteReaderQuery($"SELECT Наименование FROM Жанр", generT);
            AddComboBoxContent(generCB, generT.GenerName, null, null);
            dB.ExecuteReaderQuery($"SELECT * FROM Режиссёр", directorT);
            AddComboBoxContent(directorCB, directorT.LastName, directorT.FirstName, directorT.MiddleName);
        }
        private void AddComboBoxContent(ComboBox cb, List<string> list1, object list2, object list3)
        {
            for (int i = 0; i < list1.Count; i++)
            {
                cb.Items.Add(new CheckBox());
                if(list2 != null && list3 != null)
                    ((CheckBox)cb.Items[cb.Items.Count - 1]).Content = $"{list1[i]} {((List<string>) list2)[i]} {((List<string>)list3)[i]}";
                else
                    ((CheckBox)cb.Items[cb.Items.Count - 1]).Content = $"{list1[i]}";
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //добавление картинки
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                File.Copy(ofd.FileName, AppDomain.CurrentDomain.BaseDirectory + $@"\Images\{ofd.SafeFileName}", true);
                bmi = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + $@"\Images\{ofd.SafeFileName}", UriKind.Absolute));
                posterI.Source = bmi;
                posterFile = ofd.SafeFileName;
            }
        }
        private void FilesB_Click(object sender, RoutedEventArgs e)
        {
            //добавление видеофайлов
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            int i = 1;
            if (ofd.ShowDialog() == true)
                foreach (string file in ofd.FileNames)
                {
                    try
                    {
                        File.Copy(file, AppDomain.CurrentDomain.BaseDirectory + $@"Films\{new System.IO.FileInfo(file).Name}-№{i}.mp4", false);
                        videoLB.Items.Add($"{new System.IO.FileInfo(file).Name}-№{i}.mp4");
                        i++;
                    }
                    catch
                    {
                        MessageBox.Show("Файл с таким именем уже существует");
                    }
                }
            else MessageBox.Show("Файлы не выбраны!");
        }
        //добавление данных в таблицы БД: Жанр, Режиссёр, Актёр, Страна
        private void addGenerB_Click(object sender, RoutedEventArgs e)
        {
            table = "Жанр";
            AddSingleContent(generCB, generTB, table);
        }
        private void addDirectorB_Click(object sender, RoutedEventArgs e)
        {
            table = "Режиссёр";
            AddMultipleContent(directorCB, dirLastNTB, dirFirstNTB, dirMiddleNTB, table);
        }
        private void ActhorrB_Click(object sender, RoutedEventArgs e)
        {
            table = "Актёр";
            AddMultipleContent(acthorCB, actLastNTB, actFirstNTB, actMiddleNTB, table);
        }
        private void CountryB_Click(object sender, RoutedEventArgs e)
        {
            table = "Страна";
            AddSingleContent(countryCB, countryTB, table);
        }
        private void AddSingleContent(ComboBox CB, TextBox TB, string tbl)
        {
            CB.Items.Add(new CheckBox());
            ((CheckBox)CB.Items[CB.Items.Count - 1]).Content = $"{TB.Text}";
            dB.ExecuteQuery($"INSERT INTO [{tbl}] VALUES(N'{TB.Text}')");
            TB.Clear();
        }
        private void AddMultipleContent(ComboBox CB, TextBox lnTB, TextBox fnTB, TextBox mnTB, string tbl)
        {
            CB.Items.Add(new CheckBox());
            ((CheckBox)CB.Items[CB.Items.Count - 1]).Content = $"{lnTB.Text} {fnTB.Text} {mnTB.Text}";
            dB.ExecuteQuery($"INSERT INTO [{tbl}] VALUES(N'{lnTB.Text}', N'{fnTB.Text}', N'{mnTB.Text}')");
            lnTB.Text = "Фамилия";
            fnTB.Text = "Имя";
            mnTB.Text = "Отчество";
        }
        //проверка на заполнение полей
        private bool CheckValue()
        {
            if (Double.TryParse(ratingTB.Text, out _) && Double.TryParse(yearTB.Text, out _) && Double.TryParse(durationTB.Text, out _) && Double.TryParse(purchasePriceTB.Text, out _) && Double.TryParse(rentPriceTB.Text, out _) && posterI.Source != null && videoLB.Items.Count > 0)
                return true;
            else return false;
        }
        private void DoneB_Click(object sender, RoutedEventArgs e)
        {
            string[] nameMas = new string[3];
            if ((bool)createRB.IsChecked)//Режим Создание
            {
                if (CheckValue())
                {
                    dB.ExecuteQuery($"INSERT INTO Контент VALUES(N'{nameTB.Text}', {ratingTB.Text}, {yearTB.Text}, N'{storyTB.Text}', {durationTB.Text}, {purchasePriceTB.Text}, {rentPriceTB.Text}, N'{posterFile}')");
                    nameMas[0] = nameTB.Text;
                    nameMas[1] = storyTB.Text;
                    AddMMContent(nameMas);
                }
                else MessageBox.Show("Ошибка или отсутствие вводимых данных!");
            }
            else if ((bool)deleteRB.IsChecked)//Режим Удаление
            {
                dB.ExecuteQuery($"DELETE FROM Контент WHERE КодКонтента = {filmCB.Text.Split('№')[1]};");
            }
            else if ((bool)editRB.IsChecked)//Режим Редактирование
            {
                if (CheckValue())
                {
                    dB.ExecuteQuery($"UPDATE Контент SET Название = N'{nameTB.Text}', Рейтинг = {ratingTB.Text}, Год = {yearTB.Text}," +
                    $"Сюжет = N'{storyTB.Text}', Продолжительность = {durationTB.Text}, ЦенаПокупки = {purchasePriceTB.Text}, " +
                    $"ЦенаПроката = {rentPriceTB.Text}, Постер = N'{posterFile}' WHERE КодКонтента = {filmCB.SelectedItem.ToString().Split('№')[1]}");
                    nameMas[0] = nameTB.Text;
                    nameMas[1] = storyTB.Text;
                    AddMMContent(nameMas);
                }
                else MessageBox.Show("Ошибка или отсутствие вводимых данных!");
            }
            else
            {
                MessageBox.Show("Выберите режим работы с конструктором: \nРедактирование, Создание или Удаление!");
                return;
            }
            MessageBox.Show("Успешно!");
        }
        //заполнение БД
        private void AddMMContent(string[] nameMas)
        {
            int idCon, idEl;
            idCon = (int)dB.ExecuteScalarQuery($"SELECT КодКонтента FROM Контент WHERE Название = N'{nameMas[0]}' AND Сюжет = N'{nameMas[1]}'");
            foreach (CheckBox cb in countryCB.Items)
                if ((bool)cb.IsChecked)
                {
                    idEl = (int)dB.ExecuteScalarQuery($"SELECT КодСтраны FROM Страна WHERE Наименование = N'{cb.Content}'");
                    dB.ExecuteQuery($"INSERT INTO СтраныКонтента VALUES({idEl}, {idCon})");
                }
            foreach (CheckBox cb in acthorCB.Items)
                if ((bool)cb.IsChecked)
                {
                    nameMas = cb.Content.ToString().Split(' ');
                    idEl = (int)dB.ExecuteScalarQuery($"SELECT КодАктёра FROM Актёр WHERE Фамилия = N'{nameMas[0]}' AND Имя = N'{nameMas[1]}' AND Отчество = N'{nameMas[2]}'");
                    dB.ExecuteQuery($"INSERT INTO АктёрыКонтента VALUES({idEl}, {idCon})");
                }
            foreach (CheckBox cb in generCB.Items)
                if ((bool)cb.IsChecked)
                {
                    idEl = (int)dB.ExecuteScalarQuery($"SELECT КодЖанра FROM Жанр WHERE Наименование = N'{cb.Content}'");
                    dB.ExecuteQuery($"INSERT INTO ЖанрКонтента VALUES({idEl}, {idCon})");
                }
            foreach (CheckBox cb in directorCB.Items)
                if ((bool)cb.IsChecked)
                {
                    nameMas = cb.Content.ToString().Split(' ');
                    idEl = (int)dB.ExecuteScalarQuery($"SELECT КодРежиссёра FROM Режиссёр WHERE Фамилия = N'{nameMas[0]}' AND Имя = N'{nameMas[1]}' AND Отчество = N'{nameMas[2]}'");
                    dB.ExecuteQuery($"INSERT INTO РежиссёрыКонтента VALUES({idEl}, {idCon})");
                }
            foreach (string videoFile in videoLB.Items)
            {
                dB.ExecuteQuery($"INSERT INTO Файл VALUES(N'{videoFile}')");
                idEl = (int)dB.ExecuteScalarQuery($"SELECT [КодФайла] FROM [Файл] WHERE Ссылка = N'{videoFile}'");
                dB.ExecuteQuery($"INSERT INTO ФайлыКонтента VALUES({idEl}, {idCon})");
            }
        }
        //добавление данных о фильме на форму
        private void AddContent(string ID)
        {
            contentT = new ContentTable();
            dB.ExecuteReaderQuery($"SELECT * FROM Контент WHERE КодКонтента = {ID};", contentT);
            for(int i = 0; i < contentT.ID.Count; i++)
            {
                nameTB.Text = contentT.ContentName[i];
                ratingTB.Text = contentT.Rating[i].ToString();
                yearTB.Text = contentT.Year[i].ToString();
                storyTB.Text = contentT.Story[i];
                durationTB.Text = contentT.Duration[i].ToString();
                purchasePriceTB.Text = contentT.PurchasePrice[i].ToString();
                rentPriceTB.Text = contentT.RentalPrice[i].ToString();
                posterFile = contentT.Poster[i];
                bmi = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + $@"\Images\{posterFile}", UriKind.Absolute));
                posterI.Source = bmi;
                FileTable ft = new FileTable();
                ContentFile cf = new ContentFile();
                dB.ExecuteReaderQuery($"SELECT КодФайла FROM ФайлыКонтента WHERE КодКонтента = {ID}", cf);
                foreach (int kod in cf.DataID)
                {
                    string name = dB.ExecuteScalarQuery($"SELECT Ссылка FROM Файл WHERE КодФайла = {kod}").ToString();
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + $@"Films\{name}");
                }
                dB.ExecuteQuery($"DELETE FROM ЖанрКонтента WHERE КодКонтента = {ID};" +
                    $"DELETE FROM СтраныКонтента WHERE КодКонтента = {ID};" +
                    $"DELETE FROM АктёрыКонтента WHERE КодКонтента = {ID};" +
                    $"DELETE FROM РежиссёрыКонтента WHERE КодКонтента = {ID};" +
                    $"DELETE FROM ФайлыКонтента WHERE КодКонтента = {ID};");
            }
        }
        private void TB_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)editRB.IsChecked || (bool)deleteRB.IsChecked)
                filmCB.IsEnabled = true;
            else
                filmCB.IsEnabled = false;
        }
        //удаление данныхиз таблиц БД: Жанр, Режиссёр, Актёр, Страна
        private void FilmCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddContent(filmCB.SelectedItem.ToString().Split('№')[1]);
        }

        private void CountryDelB_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox check in countryCB.Items)
                if ((bool)check.IsChecked)
                    dB.ExecuteQuery($"DELETE FROM Страна WHERE Наименование = N'{check.Content}'");
        }

        private void GenerDelB_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox check in generCB.Items)
                if ((bool)check.IsChecked)
                    dB.ExecuteQuery($"DELETE FROM Жанр WHERE Наименование = N'{check.Content}'");
        }

        private void ActhorDelB_Click(object sender, RoutedEventArgs e)
        {
            string[] name = new string[3] { "", "", "" };
            foreach (CheckBox check in acthorCB.Items)
            {
                name = check.Content.ToString().Split(' ');
                if ((bool)check.IsChecked)
                    dB.ExecuteQuery($"DELETE FROM Актёр WHERE Фамилия = N'{name[0]}' AND Имя = N'{name[1]}' AND Отчество = N'{name[2]}'");
            }
        }

        private void DirectorDelB_Click(object sender, RoutedEventArgs e)
        {
            string[] name = new string[3] { "", "", "" };
            foreach (CheckBox check in directorCB.Items)
            {
                name = check.Content.ToString().Split(' ');
                if ((bool)check.IsChecked)
                    dB.ExecuteQuery($"DELETE FROM Режиссёр WHERE Фамилия = N'{name[0]}' AND Имя = N'{name[1]}' AND Отчество = N'{name[2]}'");
            }
        }

        private void ClearFilesB_Click(object sender, RoutedEventArgs e)
        {
            videoLB.Items.Clear();
        }
    }
}