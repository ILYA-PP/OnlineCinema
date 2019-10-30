using System;
using System.Windows;
using System.Windows.Controls;

namespace OlineCinema
{
    class ControlButton
    {
        private Button MinimizedB { get; set; }
        private Button CloseB { get; set; }
        private Window Win;
        public void CreateElements(Window w, Grid g)
        {
            Win = w;

            MinimizedB = new Button();
            CloseB = new Button();

            MinimizedB.Content = "-";
            CloseB.Content = "X";

            g.Children.Add(MinimizedB);
            g.Children.Add(CloseB);
            Grid.SetColumn(MinimizedB, g.ColumnDefinitions.Count - 2);
            Grid.SetColumn(CloseB, g.ColumnDefinitions.Count - 1);

            MinimizedB.Click += new RoutedEventHandler(MinimizedB_Click);
            CloseB.Click += new RoutedEventHandler(CloseB_Click);
        }
        private void MinimizedB_Click(object sender, EventArgs args)
        {
            Win.WindowState = WindowState.Minimized;
        }

        private void CloseB_Click(object sender, EventArgs args)
        {
            Win.Close();
        }
    }
}
