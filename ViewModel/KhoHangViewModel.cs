using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.Model;
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
    public partial class KhoHangViewModel : MainWindowTabViewModel
    {
        public KhoHangViewModel()
        {
            Task.Run(() => LoadData());
        }

        async Task LoadData()
        {
            await Task.Delay(1);

            IsLoadedComplete = true;
        }

        [RelayCommand]
        private void Hide()
        {
            if (SelectedItem == null)
                Trace.WriteLine("wtf null???");
            else
                SelectedItem.Visibility = Visibility.Collapsed;
        }

        [ObservableProperty]
        List<Product>? products;

        [ObservableProperty]
        List<BatchProduct>? batchProducts = null;

        [ObservableProperty]
        BatchProduct? selectedItem = null;
    }
}
