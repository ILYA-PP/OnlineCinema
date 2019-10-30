using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Media;

namespace OlineCinema
{
    class FilmBlock
    {
        public int ID { get; set; }
        public Border MainBorder { get; set; }
        public Border ImageBorder { get; set; }
        public Border LabelBorder { get; set; }
        public Grid MainGrid { get; set; }
        public Label FilmName { get; set; }
        public Image FilmImage { get; set; }

        public FilmBlock()
        {
            MainBorder = new Border();
            ImageBorder = new Border();
            LabelBorder = new Border();
            MainGrid = new Grid();
            FilmName = new Label();
            FilmImage = new Image();
        }

        public void CreateBlock(WrapPanel w, double wid, BitmapImage bm, string name, int id)
        {
            ID = id;
            Viewbox vb = new Viewbox();
            vb.Child = FilmName;
            MainBorder.Width = (wid - 120) / 6;
            MainBorder.Height = MainBorder.Width + 100;
            MainBorder.Margin = new Thickness(10);
            //MainBorder.BorderBrush = Brushes.Black;
            MainBorder.VerticalAlignment = VerticalAlignment.Top;
            MainBorder.BorderThickness = new Thickness(1);
            MainBorder.Child = MainGrid;

            RowDefinition rd = new RowDefinition();
            rd.Height = new GridLength(49, GridUnitType.Star);
            MainGrid.RowDefinitions.Add(rd);
            rd = new RowDefinition();
            rd.Height = new GridLength(8, GridUnitType.Star);
            MainGrid.RowDefinitions.Add(rd);
            MainGrid.Children.Add(ImageBorder);
            MainGrid.Children.Add(LabelBorder);
            Grid.SetRow(ImageBorder, 0);
            Grid.SetRow(LabelBorder, 1);

            ImageBorder.BorderBrush = Brushes.Black;
            ImageBorder.BorderThickness = new Thickness(1);
            ImageBorder.Margin = new Thickness(5,5,5,5);
            ImageBorder.Child = FilmImage;

            LabelBorder.BorderBrush = Brushes.Black;
            LabelBorder.Background = Brushes.Silver;
            LabelBorder.BorderThickness = new Thickness(1);
            LabelBorder.Margin = new Thickness(5, 3, 5, 3);
            LabelBorder.Child = vb;

            FilmName.Foreground = Brushes.Black;
            FilmName.Padding = new Thickness(1);
            FilmName.HorizontalContentAlignment = HorizontalAlignment.Center;
            FilmName.HorizontalAlignment = HorizontalAlignment.Center;
            FilmName.Margin = new Thickness(1, 0, 0, 0);

            FilmImage.Source = bm;
            FilmImage.MouseUp += FilmImage_MouseUp;
            FilmImage.Stretch = Stretch.Fill;
            FilmName.Content = name;

            w.Children.Add(MainBorder);
        }

        private void FilmImage_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Фильм film = new Фильм(ID);
            film.Show();
        }
    }
}
