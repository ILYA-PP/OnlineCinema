using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OlineCinema
{
    public partial class ЛичныйКабинет : Window
    {
        private DB dB = new DB(Properties.Settings.Default.CinemaDBConnectionString);
        private UserTable ut = new UserTable();
        private PossessionTable pt = new PossessionTable();
        private ContentTable ct = new ContentTable();
        public ЛичныйКабинет()
        {
            InitializeComponent();
            ControlButton cb = new ControlButton();
            cb.CreateElements(this, mainG);
            AddData();
        }
        private void AddData()
        {
            //добавление данных пользователя на форму
            dB.ExecuteReaderQuery($"SELECT * FROM Пользователь WHERE Логин = '{CurrentUser.Login}'", ut);
            for(int i = 0; i < ut.Login.Count; i++)
            {
                userFoto.Source = new BitmapImage(new Uri("Resources/Пользователь.jpg", UriKind.Relative));
                loginTB.Text = ut.Login[i].ToString();
                passwordPB.Password = ut.Password[i].ToString();
                openPassTB.Text = ut.Password[i].ToString();
                emailTB.Text = ut.Email[i].ToString();
                phoneTB.Text = ut.Phone[i].ToString();
                balanceTB.Text = ut.Balance[i].ToString();
                lastNameTB.Text = ut.LastName[i].ToString();
                firstNameTB.Text = ut.FirstName[i].ToString();
                middleNameTB.Text = ut.MiddleName[i].ToString();
                object r = ut.VIP[i];
                if ((bool)r) subscribeL.Content = "Подписка: Да";
                else subscribeL.Content = "Подписка: Нет";
            }
            dB.ExecuteReaderQuery($"SELECT КодКонтента, Покупка FROM Владение WHERE Логин = '{CurrentUser.Login}'", pt);
            if (pt.Purchase.Count > 0)
            {
                object name;
                for(int i = 0; i < pt.ContentID.Count; i++)
                {
                    name = dB.ExecuteScalarQuery($"SELECT Название FROM Контент WHERE КодКонтента = {pt.ContentID[i]}");
                    if (pt.Purchase[i])
                        CommentBlock.GetBlock(buySP, (string)name, "Фильм");
                    else
                        CommentBlock.GetBlock(rentSP, (string)name, "Фильм");
                }
            }
        }
        private void AddMoneyB_Click(object sender, RoutedEventArgs e)
        {
            //пополнение баланса
            double result;
            if (Double.TryParse(addMoneyTB.Text, out result))
            {
                object balance = dB.ExecuteScalarQuery($"UPDATE Пользователь SET Баланс = Баланс + {result} WHERE Логин = '{CurrentUser.Login}';" +
                    $"SELECT Баланс FROM Пользователь WHERE Логин = '{CurrentUser.Login}'"); 
                if (balance != null)
                    balanceTB.Text = balance.ToString();
            }
            else MessageBox.Show("Некорректное значение!");
        }
        private void EditB_Click(object sender, RoutedEventArgs e)
        {
            //редактирование данных
            dB.ExecuteQuery($"UPDATE Пользователь SET Пароль = N'{passwordPB.Password}', Email = N'{emailTB.Text}'," +
                    $"Телефон = N'{phoneTB.Text}', Баланс= {balanceTB.Text}, Фамилия = N'{lastNameTB.Text}'," +
                    $"Имя = N'{firstNameTB.Text}', Отчество = N'{middleNameTB.Text}'");
        }
        private void OpenPassB_Click(object sender, RoutedEventArgs e)
        {
            if (openPassTB.Visibility == Visibility.Hidden)
                openPassTB.Visibility = Visibility.Visible;
            else
                openPassTB.Visibility = Visibility.Hidden;
        }
        private void password_Changed(object sender, TextChangedEventArgs e)
        {
            if(sender is TextBox)
                passwordPB.Password = openPassTB.Text;
            else
                openPassTB.Text = passwordPB.Password;
        }
        private void password_Changed(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
                passwordPB.Password = openPassTB.Text;
            else
                openPassTB.Text = passwordPB.Password;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //выход из аккаунта
            MainWindow logIn = new MainWindow();
            logIn.Show();
            Main main = new Main();
            main.Close();
            Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Main main = new Main();
            main.Show();
        }
        private void Subscribe_Click(object sender, RoutedEventArgs e)
        {
            //оформление подписки
            if (!(bool)dB.ExecuteScalarQuery($"SELECT VIP FROM Пользователь WHERE Логин = '{CurrentUser.Login}'"))
            {
                BitmapImage bitm = new BitmapImage(new Uri("Resources/vip.jpg", UriKind.Relative));
                Оплата op = new Оплата(bitm, "Оформить подписку", 2000, "subscribe", 0);
                op.Show();
            }
            else MessageBox.Show("Подписка оформленна!");
        }
    }
}
