using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Net.Mail;
using System.Net;

namespace OlineCinema
{
    public partial class Оплата : Window
    {
        private string type;
        private int contentID;
        private DB dB = new DB(Properties.Settings.Default.CinemaDBConnectionString);
        private UserTable ut = new UserTable();
        public Оплата(BitmapImage b, string n, double p, string type, int id)
        {
            InitializeComponent();
            //добавление данных о покупке
            contentID = id;
            this.type = type;
            ControlButton cb = new ControlButton();
            cb.CreateElements(this, mainG);
            posterI.Source = b;
            nameL.Text = n;
            priceL.Text = p.ToString();
        }

        private void PayB_Click(object sender, RoutedEventArgs e)
        {
            //добавление данных о покупке в БД
            int intType;
            dB.ExecuteReaderQuery($"SELECT Баланс, Email FROM Пользователь WHERE Логин = '{CurrentUser.Login}'", ut);           
            if (ut.Balance[0] >= Double.Parse(priceL.Text))
            {
                if (type == "subscribe")
                {
                    dB.ExecuteQuery($"UPDATE Пользователь SET Баланс = {ut.Balance[0] - Double.Parse(priceL.Text)}, VIP = 1 WHERE Логин = '{CurrentUser.Login}'");
                    MessageBox.Show("Подписка оформленна!");
                    GetCheck(ut.Email[0]);
                }
                else
                {
                    if (type == "Прокат") intType = 0;
                    else intType = 1;
                    dB.ExecuteQuery($"UPDATE Пользователь SET Баланс " +
                        $"= {ut.Balance[0] - Double.Parse(priceL.Text)} WHERE Логин = '{CurrentUser.Login}';" +
                        $"INSERT INTO Владение VALUES({contentID},N'{CurrentUser.Login}',{intType})");
                    MessageBox.Show("Оплачено!");
                    GetCheck(ut.Email[0]);
                    Close();
                }
            }
            else
                MessageBox.Show("На вашем счёте недостаточно средств!");
        }

        private void GetCheck(string email)
        {
            //отправка чека на почту
            try
            {
                MailAddress from = new MailAddress("OnlineCinema2019@mail.ru", "Оплата услуг OnlineCinema");
                // кому отправляем
                MailAddress to = new MailAddress(email);
                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);
                // тема письма
                m.Subject = "Чек об оплате услуг OnlineCinema";
                // текст письма
                m.Body = $"<h2>Пользователь: {CurrentUser.Login}<br>Контент: {nameL.Text}<br>Тип: {type}<br>Сумма:{priceL.Text}<br>ОПЛАЧЕНО!<br>Желаем приятного просмотра!</h2>";
                // письмо представляет код html
                m.IsBodyHtml = true;
                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                // логин и пароль
                smtp.Credentials = new NetworkCredential("OnlineCinema2019@mail.ru", "7cq-3N2-WrW-vD3");
                smtp.EnableSsl = true;
                smtp.Send(m);
                Console.Read();
            }
            catch
            {
                MessageBox.Show("Ошибка при отправке чека!");
            }
        }
    }
}
