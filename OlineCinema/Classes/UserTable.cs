using System.Windows;
using System.Collections.Generic;

namespace OlineCinema
{
    class UserTable : IAddable
    {
        public List<string> Login { get; set; }
        public List<string> Password { get; set; }
        public List<string> Email { get; set; }
        public List<string> Phone { get; set; }
        public List<bool> VIP { get; set; }
        public List<double> Balance { get; set; }
        public List<string> LastName { get; set; }
        public List<string> FirstName { get; set; }
        public List<string> MiddleName { get; set; }

        public UserTable()
        {
            Login = new List<string>();
            Password = new List<string>();
            Email = new List<string>();
            Phone = new List<string>();
            VIP = new List<bool>();
            Balance = new List<double>();
            LastName = new List<string>();
            FirstName = new List<string>();
            MiddleName = new List<string>();
        }

        public void AddData(string name, object value)
        {
            switch (name)
            {
                case "Логин": Login.Add((string)value); break;
                case "Пароль": Password.Add((string)value); break;
                case "Email": Email.Add((string)value); break;
                case "Телефон": Phone.Add((string)value); break;
                case "VIP": VIP.Add((bool)value); break;
                case "Баланс": Balance.Add(double.Parse(value.ToString())); break;
                case "Фамилия": LastName.Add((string)value); break;
                case "Имя": FirstName.Add((string)value); break;
                case "Отчество": MiddleName.Add((string)value); break;
                default: MessageBox.Show($"Поле {name} отсутствует!"); break;
            }
        }
    }
}
