﻿using System;
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
    /// Interaction logic for DonHangView.xaml
    /// </summary>
    public partial class DonHangView : UserControl
    {
        public DonHangView()
        {
            InitializeComponent();
            DataContext = new ViewModel.DonHangViewModel();
        }
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Hide row details when clicking on the row
            ViewModel.DonHangViewModel viewModel = (ViewModel.DonHangViewModel)DataContext;
            if (sender is TextBlock)
            {
                if (viewModel.ShowDetails == DataGridRowDetailsVisibilityMode.VisibleWhenSelected)
                    viewModel.ShowDetails = DataGridRowDetailsVisibilityMode.Collapsed;
                else
                    viewModel.ShowDetails = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
            }
        }
    }
}
