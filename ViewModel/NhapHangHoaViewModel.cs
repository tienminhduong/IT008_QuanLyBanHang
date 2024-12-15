using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.Model;
using IT008_QuanLyBanHang.ViewModel.API;
using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class NhapHangHoaViewModel : ObservableObject
    {
        [ObservableProperty]
        private int? productId;

        [ObservableProperty]
        private string? batchNumber;

        [ObservableProperty]
        private int quantity;

        [ObservableProperty]
        private float importPrice;

        [ObservableProperty]
        private float sellPrice;

        [ObservableProperty]
        private DateTime? manufacturingDate;

        [ObservableProperty]
        private DateTime? expirationDate;

        public NhapHangHoaViewModel()
        {
            WeakReferenceMessenger.Default.Register<ProductSelectedMessage>(this, (r, m) =>
            {
                ProductId = m.Product.Id;

                int numberOfBatch;
                if (m.Product.Batches == null) 
                    numberOfBatch = 0;
                else
                    numberOfBatch = m.Product.Batches.Count;

                BatchNumber = $"Batch_{m.Product.Id}_{numberOfBatch + 1}";
            });
        }

        // Hủy đăng ký khi view model bị hủy
        public void Cleanup()
        {
            WeakReferenceMessenger.Default.Unregister<ProductSelectedMessage>(this);
        }

        [RelayCommand]
        private async Task AddBatch()
        {
            // Kiểm tra dữ liệu đầu vào
            if (ProductId == null || 
                string.IsNullOrWhiteSpace(BatchNumber) || 
                Quantity <= 0 || 
                ImportPrice <= 0 || 
                SellPrice <= 0 ||
                !ManufacturingDate.HasValue ||
                !ExpirationDate.HasValue)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ và đúng định dạng các thông tin.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
                
            // Tạo batch mới
            var newBatch = new Batch
            {
                Id = (int)ProductId,
                BatchNumber = BatchNumber,
                Quantity = Quantity,
                Stock = Quantity,
                ImportPrice = ImportPrice,
                Price = SellPrice,
                ManufactureDate = ManufacturingDate.Value,
                ExpirationDate = ExpirationDate.Value
            };

            try
            {
                var response = await RESTService.Instance.PostAsync("batches", new Dictionary<string, string>
                {
                    { "product_id", newBatch.Id.ToString() },
                    { "batch_number", newBatch.BatchNumber },
                    { "quantity", newBatch.Quantity.ToString() },
                    { "stock", newBatch.Stock.ToString() },
                    { "import_price", newBatch.ImportPrice.ToString() },
                    { "price", newBatch.Price.ToString() },
                    { "manufacture_date", newBatch.ManufactureDate.ToString("yyyy-MM-dd") },
                    { "expiration_date", newBatch.ExpirationDate.ToString("yyyy-MM-dd") }
                });

                if (!string.IsNullOrEmpty(response))
                {
                    Trace.WriteLine(response);
                    MessageBox.Show("Thêm lô hàng thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi thêm lô hàng.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        [RelayCommand]
        private void Cancel()
        {
            Quantity = 0;
            ImportPrice = 0;
            SellPrice = 0;
            ManufacturingDate = null;
            ExpirationDate = null;
        }
    }

    // Message class
    public class ProductSelectedMessage
    {
        public Product Product { get; }

        public ProductSelectedMessage(Product product)
        {
            Product = product;
        }
    }

}
