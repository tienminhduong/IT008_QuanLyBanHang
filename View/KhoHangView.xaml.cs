using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IT008_QuanLyBanHang.View
{
    /// <summary>
    /// Interaction logic for KhoHangView.xaml
    /// </summary>
    public partial class KhoHangView : UserControl
    {
        public KhoHangView()
        {
            InitializeComponent();
        }

        private DataGridRow? previousExpandedRow = null;


        private void ToggleIcon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Path toggleIcon)
            {
                // Find the parent DataGridRow
                var dataGridRow = FindParent<DataGridRow>(toggleIcon);
                if (dataGridRow != null)
                {
                    // Collapse the previously expanded row
                    if (previousExpandedRow != null && previousExpandedRow != dataGridRow)
                    {
                        previousExpandedRow.DetailsVisibility = Visibility.Collapsed;

                        var previousToggleIcon = FindVisualChild<Path>(previousExpandedRow, "ToggleIcon");
                        if (previousToggleIcon != null)
                        {
                            previousToggleIcon.Data = Geometry.Parse("M0,0 L5,5 L10,0 Z"); // Up arrow
                        }
                    }

                    // Toggle visibility of the current row
                    dataGridRow.DetailsVisibility =
                        dataGridRow.DetailsVisibility == Visibility.Visible
                            ? Visibility.Collapsed
                            : Visibility.Visible;

                    toggleIcon.Data = dataGridRow.DetailsVisibility == Visibility.Visible
                        ? Geometry.Parse("M0,5 L5,0 L10,5 Z")  // Down arrow
                        : Geometry.Parse("M0,0 L5,5 L10,0 Z"); // Up arrow

                    previousExpandedRow = dataGridRow;
                }

                e.Handled = true; // Prevent further bubbling of the event
            }
        }

        private T? FindVisualChild<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild && (typedChild as FrameworkElement)?.Name == name)
                {
                    return typedChild;
                }

                var foundChild = FindVisualChild<T>(child, name);
                if (foundChild != null)
                {
                    return foundChild;
                }
            }
            return null;
        }

        private T? FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (child != null)
            {
                if (child is T parent)
                {
                    return parent;
                }
                child = VisualTreeHelper.GetParent(child);
            }
            return null;
        }

    }
}
