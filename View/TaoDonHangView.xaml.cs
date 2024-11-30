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
using System.Windows.Shapes;

namespace IT008_QuanLyBanHang.View
{
    /// <summary>
    /// Interaction logic for TaoDonHangView.xaml
    /// </summary>
    public partial class TaoDonHangView : Window
    {
        public TaoDonHangView()
        {
            InitializeComponent();
            DataContext = new ViewModel.TaoDonHangViewModel();
        }
    }
}
