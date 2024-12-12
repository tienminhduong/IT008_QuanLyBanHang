using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.Interfaces;
using IT008_QuanLyBanHang.Model;
using IT008_QuanLyBanHang.ViewModel.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static MaterialDesignThemes.Wpf.Theme;

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

            this.PropertyChanged += OnViewModelPropertyChanged;

            this.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(SelectedProduct))
                {
                    await SelectProduct(SelectedProduct);
                } 
                    
            };
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SearchText):
                    FilterData();
                    break;
                case nameof(SelectedTabIndex):
                    OnSelectedTabIndexChanged(SelectedTabIndex);
                    break;
            }
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
            BatchProducts = await ProductAPI.GetAllProductsWithBatches();
            var p = await ProductAPI.GetAllProducts();
            if (p != null)
                Products = new ObservableCollection<Product>(p);


            // Chỉnh thứ tự lúc đầu theo tăng dần
            originalDataList = new ObservableCollection<Product>(
                Products?.OrderBy(p => p.Id)
            );

            Products = new ObservableCollection<Product>(originalDataList);
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

        [ObservableProperty]
        Product? selectedProduct;

        [ObservableProperty]
        ObservableCollection<Batch>? productBatches;

        [ObservableProperty]
        private int selectedTabIndex;

        [RelayCommand]
        private async Task SelectProduct(Product? product)
        {
            if (product == null)
                return;
            SelectedProduct = product;
            ProductBatches = new ObservableCollection<Batch>(await ProductAPI.GetBatchesOfProduct(SelectedProduct));
            Trace.WriteLine($"Selected Product: {SelectedProduct?.ProductName}, Batches Count: {ProductBatches?.Count}");
        }

        partial void OnSelectedTabIndexChanged(int value)
        {
            if (originalDataList == null) return;

            switch (value)
            {
                case 0: // "Tất cả" tab
                    Products = new ObservableCollection<Product>(originalDataList);
                    break;
                case 1: // "Đang trưng bày" tab
                    Products = new ObservableCollection<Product>(
                        originalDataList.Where(p => p.Status == "active")
                    );
                    break;
                default:
                    Products = new ObservableCollection<Product>(originalDataList);
                    break;
            }
        }
    }
}
