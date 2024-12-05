using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.Interfaces;
using IT008_QuanLyBanHang.Model;
using IT008_QuanLyBanHang.ViewModel.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class KhoHangViewModel : ObservableObject, ITabViewModel
    {
        public KhoHangViewModel()
        {
        }

        [RelayCommand]
        public async Task LoadData()
        {
            BatchProducts = await ProductAPI.GetAllProductsWithBatches();
        }

        [RelayCommand]
        private void Hide()
        {
            if (SelectedItem == null)
                Trace.WriteLine("wtf null???");
            else
                SelectedItem.Visibility = Visibility.Collapsed;
        }


        [RelayCommand]
        private void TestLoad()
        {
            Trace.WriteLine("TestLoad");
        }

        [ObservableProperty]
        List<BatchProduct> batchProducts = new();

        [ObservableProperty]
        BatchProduct? selectedItem = null;
    }
}
