using System.Windows;
using System.Windows.Controls;

namespace OlineCinema
{
    public partial class Регистрация : Window
    {
        public Регистрация()
        {
            InitializeComponent();
            ControlButton cb = new ControlButton();
            cb.CreateElements(this, mainG);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //добавление данных о пользователе в БД, если все данные введены верно
            if (PasswordTB.Password == RepPasswordTB.Password)
            {
                DB dB = new DB(Properties.Settings.Default.CinemaDBConnectionString);
                object login = dB.ExecuteScalarQuery($"SELECT Логин FROM Пользователь WHERE Логин = '{LoginTB.Text}'");
                if (login == null)
                {
                    int rows = dB.ExecuteQuery($"INSERT INTO [Пользователь] Values(N'{LoginTB.Text}', N'{PasswordTB.Password}', N'{EmailTB.Text}', N'{PhoneTB.Text}', 0, 0, N'{LastNameTB.Text}', N'{FirstNameTB.Text}', N'{MiddleNameTB.Text}')");
                    if (rows == 1)
                    {
                        MessageBox.Show("Регистрация прошла успешно!");
                        LastNameTB.Clear();
                        FirstNameTB.Clear();
                        MiddleNameTB.Clear();
                        EmailTB.Clear();
                        PhoneTB.Clear();
                        LoginTB.Clear();
                        PasswordTB.Clear();
                        RepPasswordTB.Clear();
                        AddText();
                    }
                }
                else MessageBox.Show("Такой логин уже существует!");
            }
            else MessageBox.Show("Пароли не совпадают!");
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AddText();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                ((TextBox)sender).Text = "";
            }
            catch
            {
                ((PasswordBox)sender).Password = "";
            }
        }

        private void AddText()
        {
            //очистка формы
            if (LastNameTB.Text == "")
                LastNameTB.Text = "Фамилия";
            if (FirstNameTB.Text == "")
                FirstNameTB.Text = "Имя";
            if (MiddleNameTB.Text == "")
                MiddleNameTB.Text = "Отчество";
            if (EmailTB.Text == "")
                EmailTB.Text = "Email";
            if (PhoneTB.Text == "")
                PhoneTB.Text = "Телефон";
            if (LoginTB.Text == "")
                LoginTB.Text = "Логин";
            if (PasswordTB.Password == "")
                PasswordTB.Password = "Пароль";
            if (RepPasswordTB.Password == "")
                RepPasswordTB.Password = "Пароль";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //открытие окна Авторизация
            MainWindow log = new MainWindow();
            log.Show();
            Close();
        }
    }
}
