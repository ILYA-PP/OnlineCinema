using System.Windows;
using System.Collections.Generic;

namespace OlineCinema
{
    class CountryTable : IAddable
    {
        public List<int> ID { get; set; }
        public List<string> CountryName { get; set; }
        public CountryTable()
        {
            ID = new List<int>();
            CountryName = new List<string>();
        }

        public void AddData(string name, object value)
        {
            switch (name)
            {
                case "КодСтраны": ID.Add((int)value); break;
                case "Наименование": CountryName.Add((string)value); break;
                default: MessageBox.Show($"Поле {name} отсутствует!"); break;
            }
        }
    }
}
