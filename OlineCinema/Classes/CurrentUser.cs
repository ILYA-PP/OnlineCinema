using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OlineCinema
{
    class CurrentUser
    {
        private static string login;
        public static string Login
        {
            get
            {
                return login;
            }
            set
            {
                if (value != "")
                    login = value;
                else
                    MessageBox.Show("Логин не может быть пустым!");
            }
        }
    }
}
