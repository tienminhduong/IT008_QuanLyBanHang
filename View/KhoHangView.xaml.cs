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

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid grid && grid.SelectedItem != null)
            {
                var selectedRow = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;

                if (selectedRow != null)
                {
                    if (previousExpandedRow != null && previousExpandedRow != selectedRow)
                    {
                        previousExpandedRow.DetailsVisibility = Visibility.Collapsed;
                        
                        var previousToggleIcon = FindVisualChild<Path>(previousExpandedRow, "ToggleIcon");
                        if (previousToggleIcon != null)
                        {
                            previousToggleIcon.Data = Geometry.Parse("M0,0 L5,5 L10,0 Z"); // Up arrow
                        }
                    }

                    selectedRow.DetailsVisibility =
                        selectedRow.DetailsVisibility == Visibility.Visible
                            ? Visibility.Collapsed
                            : Visibility.Visible;

                    var toggleIcon = FindVisualChild<Path>(selectedRow, "ToggleIcon");
                    if (toggleIcon != null)
                    {
                        toggleIcon.Data = selectedRow.DetailsVisibility == Visibility.Visible
                            ? Geometry.Parse("M0,5 L5,0 L10,5 Z")  // Down arrow
                            : Geometry.Parse("M0,0 L5,5 L10,0 Z"); // Up arrow
                    }

                    previousExpandedRow = selectedRow;
                }

                grid.SelectedItem = null;
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

    }
}
