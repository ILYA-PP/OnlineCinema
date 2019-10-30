using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlineCinema
{
    class PossessionTable : IAddable
    {
        public List<int> ID { get; set; }
        public List<int> ContentID { get; set; }
        public List<string> Login { get; set; }
        public List<bool> Purchase { get; set; }

        public PossessionTable()
        {
            ID = new List<int>();
            ContentID = new List<int>();
            Login = new List<string>();
            Purchase = new List<bool>();
        }

        public void AddData(string name, object value)
        {
            switch (name)
            {
                case "КодЗаписи": ID.Add((int)value); break;
                case "КодКонтента": ContentID.Add((int)value); break;
                case "Логин": Login.Add((string)value); break;
                case "Покупка": Purchase.Add((bool)value); break;
                default: MessageBox.Show($"Поле {name} отсутствует!"); break;
            }
        }
    }
}
