using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.Model;
using IT008_QuanLyBanHang.ViewModel.API;
using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows;

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
        private decimal importPrice;

        [ObservableProperty]
        private decimal sellPrice;

        [ObservableProperty]
        private DateTime? manufacturingDate;

        [ObservableProperty]
        private DateTime? expirationDate;

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
                ImportPrice = ImportPrice.ToString(),
                Price = SellPrice.ToString(),
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
                    { "import_price", newBatch.ImportPrice },
                    { "price", newBatch.Price },
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
    }
}
