using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlineCinema
{
    class GenerTable : IAddable
    {
        public List<int> ID { get; set; }
        public List<string> GenerName { get; set; }
        public GenerTable()
        {
            ID = new List<int>();
            GenerName = new List<string>();
        }

        public void AddData(string name, object value)
        {
            switch (name)
            {
                case "КодЖанра": ID.Add((int)value); break;
                case "Наименование": GenerName.Add((string)value); break;
                default: MessageBox.Show($"Поле {name} отсутствует!"); break;
            }
        }
    }
}
