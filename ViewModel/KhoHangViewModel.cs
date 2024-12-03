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
            //LoadDataCommand = new AsyncRelayCommand(LoadData);
            Task.Run(() => LoadData());
        }

        //public IAsyncRelayCommand LoadDataCommand { get; }


        //public async Task LoadData()
        //{
        //    //string temp = await RESTService.Instance.GetAsyncOfType("products");
        //    //Trace.WriteLine(temp);
        //    ProductAPI? productResponse = JsonSerializer.Deserialize<ProductAPI>(temp);
        //    //if (productResponse?.Data?.Items != null)
        //        //Products = productResponse.Data.Items;

        //    if (Products != null)
        //    {
        //        BatchProducts = new();
        //        Trace.WriteLine($"Product size: {Products.Count}");
        //        foreach (Product p in Products)
        //        {
        //            if (p.Batches != null)
        //            {
        //                foreach (var b in p.Batches)
        //                {
        //                    BatchProduct bp = new(p, b);
        //                    BatchProducts.Add(bp);
        //                }
        //            }
        //        }
        //    }
        //}

        public async Task LoadData()
        {
            await Task.Delay(1);
            foreach (Product p in productAPI.Products)
            {
                if (p.Batches != null)
                {
                    foreach (var b in p.Batches)
                    {
                        BatchProduct bp = new(p, b);
                        BatchProducts.Add(bp);
                    }
                }
            }
        }

        [RelayCommand]
        private void Hide()
        {
            if (SelectedItem == null)
                Trace.WriteLine("wtf null???");
            else
                SelectedItem.Visibility = Visibility.Collapsed;
        }

        //[ObservableProperty]
        //List<Product>? products;
        ProductAPI productAPI = new();

        [ObservableProperty]
        ObservableCollection<BatchProduct> batchProducts = new();

        [ObservableProperty]
        BatchProduct? selectedItem = null;
    }
}
