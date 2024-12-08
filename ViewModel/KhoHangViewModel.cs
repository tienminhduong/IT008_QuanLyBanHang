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
        [ObservableProperty]
        private string searchText = string.Empty;

        private ObservableCollection<Product>? originalDataList;

        public KhoHangViewModel()
        {
            LoadDataCommand = new AsyncRelayCommand(LoadData);
            Task.Run(() => LoadData());

            this.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SearchText))
                {
                    FilterData();
                }
            };
        }

        private void FilterData()
        {
            if (originalDataList == null)
                return;

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Products = new ObservableCollection<Product>(originalDataList);
            }
            else
            {
                // Tìm kiếm dưới dạng số nguyên nếu SearchText là số
                bool isNumeric = int.TryParse(SearchText, out int searchNumber);

                var filteredList = originalDataList.Where(item =>
                    item.ProductName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
                    item.Category != null && item.Category.CategoryName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
                    (isNumeric && item.Id == searchNumber)
                );
                Products = new ObservableCollection<Product>(filteredList);
            }
        }


        public IAsyncRelayCommand LoadDataCommand { get; }

        [RelayCommand]
        private void Expand(BatchProduct batchProduct)
        {
            if (batchProduct != null)
            {
                batchProduct.IsExpanded = !batchProduct.IsExpanded;
                Trace.WriteLine($"IsExpanded: {batchProduct.IsExpanded}");
            }
        }

        public async Task LoadData()
        {
            //string temp = await RESTService.Instance.GetAsync("products");
            //Trace.WriteLine(temp);
            //ProductResponse? productResponse = JsonSerializer.Deserialize<ProductResponse>(temp);
            //if (productResponse?.Data?.Items != null)
            //    Products =  new ObservableCollection<Product>(productResponse.Data.Items);

            ///*if (Products != null)
            //{
            //    BatchProducts = new();
            //    Trace.WriteLine($"Product size: {Products.Count}");
            //    foreach (Product p in Products)
            //    {
            //        if (p.Batches != null)
            //        {
            //            foreach (var b in p.Batches)
            //            {
            //                BatchProduct bp = new(p, b);
            //                BatchProducts.Add(bp);
            //            }
            //        }
            //    }
            //}*/

            //// Order by Product ID
            //originalDataList = new ObservableCollection<Product>(
            //    Products?.OrderBy(p => p.Id)
            //);

            //Products = new ObservableCollection<Product>(originalDataList);
            BatchProducts = await ProductAPI.GetAllProductsWithBatches();
            var p = await ProductAPI.GetAllProducts();
            if (p != null)
                Products = new ObservableCollection<Product>(p);
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
        private void AddProduct()
        {
            var taoHangHoaView = new IT008_QuanLyBanHang.View.TaoHangHoa();
            taoHangHoaView.Show();
        }

        [ObservableProperty]
        ObservableCollection<Product>? products;

        [ObservableProperty]
        List<BatchProduct> batchProducts = new();

        [ObservableProperty]
        BatchProduct? selectedItem = null;

    }
}
