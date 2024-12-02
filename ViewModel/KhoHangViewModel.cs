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
        public KhoHangViewModel()
        {
            LoadSampleData();
            LoadDataCommand = new AsyncRelayCommand(LoadData);
            Task.Run(() => LoadData());
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
        ObservableCollection<BatchProduct> batchProducts = new();

        [ObservableProperty]
        BatchProduct? selectedItem = null;

        private void LoadSampleData()
        {
            // Tạo dữ liệu mẫu
            Products = new List<Product>
            {
                new Product
                {
                    Id = 001,
                    ProductName = "Sản phẩm 1",
                    Category = new Category { CategoryName = "Loại 1" },
                    Batches = new List<Batch>
                    {
                        new Batch
                        {
                            BatchNumber = "B001",
                            Quantity = 10,
                            Price = "10000",
                            ManufactureDate = DateTime.Now.AddMonths(-1),
                            ExpirationDate = DateTime.Now.AddMonths(5),
                        }
                    }
                },
                new Product
                {
                    Id = 002,
                    ProductName = "Sản phẩm 2",
                    Category = new Category { CategoryName = "Loại 2" },
                    Batches = new List<Batch>
                    {
                        new Batch
                        {
                            BatchNumber = "B002",
                            Quantity = 5,
                            Price = "20000",
                            ManufactureDate = DateTime.Now.AddMonths(-2),
                            ExpirationDate = DateTime.Now.AddMonths(6),
                        }
                    }
                }
            };

            // Chuyển Products thành BatchProducts
            if (Products != null)
            {
                BatchProducts = new ObservableCollection<BatchProduct>(
                    Products.SelectMany(p => p.Batches.Select(b => new BatchProduct(p, b)))
                );
            }
        }

    }
}
