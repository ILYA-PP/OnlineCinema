using System.Windows;
using System.Collections.Generic;

namespace OlineCinema
{
    class ContentFile : ManyToManyTable
    {
        public override void AddData(string name, object value)
        {
            switch (name)
            {
                case "КодФайла": DataID.Add((int)value); break;
                case "КодКонтента": ContentID.Add((int)value); break;
                default: MessageBox.Show($"Поле {name} отсутствует!"); break;
            }
        }
    }
}
