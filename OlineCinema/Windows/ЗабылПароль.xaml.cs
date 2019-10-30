using System.Windows;
using System.Net.Mail;
using System.Net;

namespace OlineCinema
{
    public partial class ЗабылПароль : Window
    { 
        public ЗабылПароль()
        {
            InitializeComponent();
            ControlButton cb = new ControlButton();
            cb.CreateElements(this, mainG);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //открытие окна Авторизация
            MainWindow logIn = new MainWindow();
            logIn.Show();
            Close();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //отправка пороля на почту, если электронная почта привязана к аккаунту
            DB dB = new DB(Properties.Settings.Default.CinemaDBConnectionString);
            object password = dB.ExecuteScalarQuery($"SELECT Пароль FROM Пользователь WHERE Email = N'{mailTB.Text}'");
            if (password != null)
            {
                try
                {
                    // отправитель - устанавливаем адрес и отображаемое в письме имя
                    MailAddress from = new MailAddress("OnlineCinema2019@mail.ru", "Восстановление пароля OnlineCinema");
                    // кому отправляем
                    MailAddress to = new MailAddress(mailTB.Text);
                    // создаем объект сообщения
                    MailMessage m = new MailMessage(from, to);
                    // тема письма
                    m.Subject = "Восстановление пароля OnlineCinema";
                    // текст письма
                    m.Body = "<h2>Пароль от вашего аккаунта: " + $"{password.ToString()}<br>Желаем приятного просмотра!" + "</h2>";
                    // письмо представляет код html
                    m.IsBodyHtml = true;
                    // адрес smtp-сервера и порт, с которого будем отправлять письмо
                    SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                    // логин и пароль
                    smtp.Credentials = new NetworkCredential("OnlineCinema2019@mail.ru", "7cq-3N2-WrW-vD3");
                    smtp.EnableSsl = true;
                    smtp.Send(m);
                    MessageBox.Show("Пароль отправлен на ваш электронный адрес!");
                }
                catch
                {
                    MessageBox.Show("Ошибка при отправке пароля!");
                } 
            }
            else MessageBox.Show("Данная почта не прикрепленна ни к одному аккаунту!");
        }
    }
}
