using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlineCinema
{
    class DirectorTable : IAddable
    {
        public List<int> ID { get; set; }
        public List<string> LastName { get; set; }
        public List<string> FirstName { get; set; }
        public List<string> MiddleName { get; set; }

        public DirectorTable()
        {
            ID = new List<int>();
            LastName = new List<string>();
            FirstName = new List<string>();
            MiddleName = new List<string>();
        }

        public void AddData(string name, object value)
        {
            switch (name)
            {
                case "КодРежиссёра": ID.Add((int)value); break;
                case "Фамилия": LastName.Add((string)value); break;
                case "Имя": FirstName.Add((string)value); break;
                case "Отчество": MiddleName.Add((string)value); break;
                default: MessageBox.Show($"Поле {name} отсутствует!"); break;
            }
        }

    }
}
