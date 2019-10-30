using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlineCinema
{
    class ContentTable : IAddable
    {
        public List<int> ID { get; set; }
        public List<string> ContentName { get; set; }
        public List<double> Rating { get; set; }
        public List<int> Year { get; set; }
        public List<string> Story { get; set; }
        public List<int> Duration { get; set; }
        public List<double> PurchasePrice { get; set; }
        public List<double> RentalPrice { get; set; }
        public List<string> Poster { get; set; }

        public ContentTable()
        {
            ID = new List<int>();
            ContentName = new List<string>();
            Rating = new List<double>();
            Year = new List<int>();
            Story = new List<string>();
            Duration = new List<int>();
            PurchasePrice = new List<double>();
            RentalPrice = new List<double>();
            Poster = new List<string>();
        }

        public void AddData(string name, object value)
        {
            switch (name)
            {
                case "КодКонтента": ID.Add((int)value); break;
                case "Название": ContentName.Add((string)value); break;
                case "Рейтинг": Rating.Add(double.Parse(value.ToString())); break;
                case "Год": Year.Add((int)value); break;
                case "Сюжет": Story.Add((string)value); break;
                case "Продолжительность": Duration.Add((int)value); break;
                case "ЦенаПокупки": PurchasePrice.Add(double.Parse(value.ToString())); break;
                case "ЦенаПроката": RentalPrice.Add(double.Parse(value.ToString())); break;
                case "Постер": Poster.Add((string)value); break;
                default: MessageBox.Show($"Поле {name} отсутствует!"); break;
            }
        }
    }
}
