using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlineCinema
{
    class FileTable : IAddable
    {
        public List<int> ID { get; set; }
        public List<string> Reference { get; set; }

        public FileTable()
        {
            ID = new List<int>();
            Reference = new List<string>();
        }
        public void AddData(string name, object value)
        {
            switch (name)
            {
                case "КодФайла": ID.Add((int)value); break;
                case "Ссылка": Reference.Add((string)value); break;
                default: MessageBox.Show($"Поле {name} отсутствует!"); break;
            }
        }
    }
}
