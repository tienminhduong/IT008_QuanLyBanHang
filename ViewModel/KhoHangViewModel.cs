using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.Interfaces;
using IT008_QuanLyBanHang.Model;
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

        private ObservableCollection<BatchProduct>? originalDataList;

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
                BatchProducts = new ObservableCollection<BatchProduct>(originalDataList);
            }
            else
            {
                // Tìm kiếm dưới dạng số nguyên nếu SearchText là số
                bool isNumeric = int.TryParse(SearchText, out int searchNumber);

                var filteredList = originalDataList.Where(item =>
                    item.Product?.ProductName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
                    item.Product?.Category != null && item.Product.Category.CategoryName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
                    (isNumeric && item.Product?.Id == searchNumber) ||
                    item.Batch.ManufactureDate.ToString("d").Contains(SearchText) ||
                    item.Batch.ExpirationDate.ToString("d").Contains(SearchText)
                );
                BatchProducts = new ObservableCollection<BatchProduct>(filteredList);
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
            string temp = await RESTService.Instance.GetAsync("products");
            Trace.WriteLine(temp);
            ProductResponse? productResponse = JsonSerializer.Deserialize<ProductResponse>(temp);
            if (productResponse?.Data?.Items != null)
                Products = productResponse.Data.Items;

            if (Products != null)
            {
                BatchProducts = new();
                Trace.WriteLine($"Product size: {Products.Count}");
                foreach (Product p in Products)
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

            // Order by Product ID
            originalDataList = new ObservableCollection<BatchProduct>(
                BatchProducts.OrderBy(bp => bp.Product?.Id)
            );

            BatchProducts = new ObservableCollection<BatchProduct>(originalDataList);
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
        private void Add()
        {
            var taoHangHoaView = new IT008_QuanLyBanHang.View.TaoHangHoa();
            taoHangHoaView.Show();
        }

        [ObservableProperty]
        List<Product>? products;

        [ObservableProperty]
        ObservableCollection<BatchProduct> batchProducts = new();

        [ObservableProperty]
        BatchProduct? selectedItem = null;

    }
}
