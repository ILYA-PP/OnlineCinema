using System;
using System.Threading;
using System.Windows;

namespace OlineCinema
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.SetData("DataDirectory", Environment.CurrentDirectory.Replace(@"\bin\Debug", ""));
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            //Добавление кнопок Закрыть/Свернуть
            ControlButton cb = new ControlButton();
            cb.CreateElements(this, mainG);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Открытие окна Главная, если логин и пароль введены верно
            if (LoginTB.Text != "" || PasswordTB.Password != "")
            {
                DB dB = new DB(Properties.Settings.Default.CinemaDBConnectionString);
                object result = dB.ExecuteScalarQuery($"SELECT * FROM [Пользователь] " +
                    $"WHERE [Логин] = N'{LoginTB.Text}' AND [Пароль] = N'{PasswordTB.Password}'");
                if(result != null)
                {
                    CurrentUser.Login = LoginTB.Text;
                    Main main = new Main();
                    main.Show();
                    Close();
                }
                else MessageBox.Show("Неверный логин или пароль!");                    
            }
            else MessageBox.Show("Есть незаполненные поля!");
        }
        private void SignInB_Click(object sender, RoutedEventArgs e)
        {
            //Открытие окна Регистрация
            Регистрация reg = new Регистрация();
            reg.Show();
            Close();
        }
        private void LoginTB_GotFocus(object sender, RoutedEventArgs e)
        {
            LoginTB.Clear();
        }
        private void LoginTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LoginTB.Text == "")
                LoginTB.Text = "Логин";
        }
        private void PasswordTB_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordTB.Clear();
        }
        private void PasswordTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordTB.Password == "")
                PasswordTB.Password = "Пароль";
        }
        private void DontKnowPassB_Click(object sender, RoutedEventArgs e)
        {
            //Открытие окна Забыл пароль
            ЗабылПароль Pass = new ЗабылПароль();
            Pass.Show();
            Close();
        }
        private void ContentFormB_Click(object sender, RoutedEventArgs e)
        {
            //Открытие окна Контентмейкер
            Контентмейкер cont= new Контентмейкер();
            cont.Show();
            Close();
        }
    }
}
