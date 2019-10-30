using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OlineCinema
{
    class CommentBlock
    {
        public static  Border MainBorder { get; set; }
        public static TextBlock ContentBlock { get; set; }

        public static void GetBlock(StackPanel sp, string text, string login)
        {
            MainBorder = new Border();
            ContentBlock = new TextBlock();

            MainBorder.BorderBrush = Brushes.Black;
            MainBorder.BorderThickness = new Thickness(1);
            MainBorder.Margin = new Thickness(3);

            ContentBlock.TextWrapping = TextWrapping.Wrap;
            ContentBlock.Text = $"{login}:\n{text}";
            MainBorder.Child = ContentBlock;
            sp.Children.Add(MainBorder);
        }
    }
}
