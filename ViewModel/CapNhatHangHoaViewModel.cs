using CloudinaryDotNet;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IT008_QuanLyBanHang.DTOs;
using IT008_QuanLyBanHang.Model;
using IT008_QuanLyBanHang.ViewModel.API;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class CapNhatHangHoaViewModel: ObservableObject
    {
        [ObservableProperty]
        private int? productId;

        [ObservableProperty]
        private string? productName;

        [ObservableProperty]
        private string? productUnit;

        [ObservableProperty]
        private int? productCategory;

        [ObservableProperty]
        private ObservableCollection<Category>? categoriesList = new ObservableCollection<Category>();

        private ProductDTO originalProduct;

        [RelayCommand]
        void UploadImage()
        {
            // savefiledialog for selecting image
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                // upload image to cloudinary
                originalProduct.image_url = ImageUrl = openFileDialog.FileName;
            }
        }

        [ObservableProperty] string imageUrl = "";

        public CapNhatHangHoaViewModel()
        {
            try
            {
                Task.Run(() => PopulateComboBox());
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Lỗi: {ex.Message}");
            }

            WeakReferenceMessenger.Default.Register(this, (MessageHandler<object, ProductSelectedMessage>)((r, m) =>
            {
                this.ProductId = m.Product.Id;
                ProductName = m.Product.ProductName;
                ProductUnit = m.Product.Unit;
                ProductCategory = m.Product.Category.Id;

                originalProduct = new ProductDTO
                {
                    id = m.Product.Id,
                    product_name = m.Product.ProductName,
                    unit = m.Product.Unit,
                    image_url = m.Product.ImageURL,
                    status = m.Product.Status,
                    category_id = m.Product.Category.Id,
                };
            }));
        }

        // Hủy đăng ký khi view model bị hủy
        public void Cleanup()
        {
            WeakReferenceMessenger.Default.Unregister<ProductSelectedMessage>(this);
        }


        public async Task PopulateComboBox()
        {
            CategoriesList = new ObservableCollection<Category>(await CategoryAPI.GetAllCategories());
        }



        [RelayCommand]
        private async Task UpdateProduct()
        {
            if (string.IsNullOrWhiteSpace(ProductName) || ProductCategory == null || string.IsNullOrWhiteSpace(ProductUnit))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var productData = new Dictionary<string, string>
            {
                { "id", originalProduct.id.ToString() },
                { "product_name", ProductName },
                { "category_id", ProductCategory.ToString()},
                { "status", "active" },
                { "image_url", originalProduct.image_url ?? ""},
                { "unit", ProductUnit }
            };

            try
            {
                string endpoint = $"products/{originalProduct.id}";
                string result = await RESTService.Instance.PutAsync(endpoint, productData);

                if (!string.IsNullOrEmpty(result))
                {
                    MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại. Vui lòng thử lại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật sản phẩm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine($"Lỗi UpdateProduct: {ex.Message}");
            }
        }

        [RelayCommand]
        private void Reset()
        {
            ProductName = originalProduct.product_name;
            ProductUnit = originalProduct.unit;
            ProductCategory = originalProduct.category_id;
        }
    }
}
