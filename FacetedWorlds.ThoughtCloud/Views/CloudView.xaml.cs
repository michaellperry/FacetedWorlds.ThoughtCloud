using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FacetedWorlds.ThoughtCloud.Views
{
    public partial class CloudView : UserControl
    {
        public CloudView()
        {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textBox = sender as TextBox;
                if (textBox != null)
                {
                    textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                }
            }
        }

        private void TextBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue as bool? == true)
            {
                TextBox textBox = sender as TextBox;
                if (textBox != null)
                {
                    textBox.Focus();
                    textBox.SelectAll();
                }
            }
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Grid grid = sender as Grid;
            RectangleGeometry geometry = grid.Clip as RectangleGeometry;
            if (geometry == null)
            {
                geometry = new RectangleGeometry();
                grid.Clip = geometry;
            }
            geometry.Rect = new Rect(new Point(0.0, 0.0), new Size(grid.ActualWidth, grid.ActualHeight));
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.IsEnabled)
            {
                textBox.Focus();
                textBox.SelectAll();
            }
        }
    }
}
