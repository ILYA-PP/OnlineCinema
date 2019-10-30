using System.Windows;

namespace OlineCinema
{
    class ContentActhor : ManyToManyTable
    {
        public override void AddData(string name, object value)
        {
            switch (name)
            {
                case "КодАктёра": DataID.Add((int)value); break;
                case "КодКонтента": ContentID.Add((int)value); break;
                default: MessageBox.Show($"Поле {name} отсутствует!"); break;
            }
        }
    }
}
