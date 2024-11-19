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
            string temp = await RESTService.Instance.GetAsync("products");
            ProductResponse? productResponse = JsonSerializer.Deserialize<ProductResponse>(temp);
            if (productResponse?.Data?.Items != null)
                Products = productResponse.Data.Items;

            if (Products != null)
                foreach (Product p in Products)
                {
                    if (p.Batches != null)
                    {
                        foreach (var b in p.Batches)
                        {
                            BatchProduct bp = new(p, b);
                            BatchProducts ??= new();
                            BatchProducts.Add(bp);
                        }
                    }
                }

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
