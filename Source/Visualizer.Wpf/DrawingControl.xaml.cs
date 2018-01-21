using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Stride.Visualizer.Wpf
{
    public partial class DrawingControl : UserControl
    {
        public DrawingControl()
        {
            InitializeComponent();
        }

        public void AddDrawing(Drawing drawing)
        {
            var row = AddGridRow();
            var image = AddImage(row);
            image.Source = new DrawingImage(drawing);
        }

        Image AddImage(int row)
        {
            var image = new Image
            {
                Stretch = Stretch.None,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            Grid.SetRow(image, row);
            Grid.Children.Add(image);
            return image;
        }

        int AddGridRow()
        {
            var rowDefinition = new RowDefinition {Height = GridLength.Auto};
            var rows = Grid.RowDefinitions;
            var index = rows.Count;
            rows.Add(rowDefinition);
            return index;
        }
    }
}
