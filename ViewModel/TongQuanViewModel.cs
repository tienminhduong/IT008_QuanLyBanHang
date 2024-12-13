using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.Interfaces;
using IT008_QuanLyBanHang.Model;
using IT008_QuanLyBanHang.ViewModel.API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class TongQuanViewModel : ObservableObject, ITabViewModel
    {
        public TongQuanViewModel()
        {
            Task.Run(() => LoadData());
        }

        public async Task LoadData()
        {
            //await ProductAPI.GetAllProducts();
        }
    }
}
