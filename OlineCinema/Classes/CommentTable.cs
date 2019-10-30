using System.Windows;
using System.Collections.Generic;

namespace OlineCinema
{
    class CommentTable : IAddable
    {
        public List<int> ID { get; set; }
        public List<int> ContentID { get; set; }
        public List<string> Login { get; set; }
        public List<string> Text { get; set; }

        public CommentTable()
        {
            ID = new List<int>();
            ContentID = new List<int>();
            Login = new List<string>();
            Text = new List<string>();
        }
        public void AddData(string name, object value)
        {
            switch (name)
            {
                case "КодОтзыва": ID.Add((int)value); break;
                case "КодКонтента": ContentID.Add((int)value); break;
                case "Логин": Login.Add((string)value); break;
                case "Текст": Text.Add((string)value); break;
                default: MessageBox.Show($"Поле {name} отсутствует!"); break;
            }
        }
    }
}
