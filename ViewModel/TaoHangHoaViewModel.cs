using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using IT008_QuanLyBanHang.Model;
using System.Diagnostics;
using System.Collections.ObjectModel;
using IT008_QuanLyBanHang.ViewModel.API;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class TaoHangHoaViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? productName;

        [ObservableProperty]
        private string? productUnit;

        [ObservableProperty]
        private int? productCategory;

        [ObservableProperty]
        private ObservableCollection<Category>? categoriesList = new ObservableCollection<Category>();

        public TaoHangHoaViewModel()
        {
            try
            {
                PopulateComboBox();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Lỗi: {ex.Message}");
            }
        }


        public async Task PopulateComboBox()
        {
            CategoriesList = new ObservableCollection<Category>(await CategoryAPI.GetAllCategories());
        }



        [RelayCommand]
        private async Task AddProduct()
        {
            if (string.IsNullOrWhiteSpace(ProductName) || ProductCategory == null || string.IsNullOrWhiteSpace(ProductUnit))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var productData = new Dictionary<string, string>
            {
                { "product_name", ProductName },
                { "category_id", ProductCategory.ToString()},
                { "status", "active" },
                { "unit", ProductUnit }
            };

            try
            {
                var response = await RESTService.Instance.PostAsync("products", productData);
                if (!string.IsNullOrEmpty(response))
                {
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    Trace.WriteLine(response);
                }
                else
                {
                    MessageBox.Show("Thêm sản phẩm thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm sản phẩm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            ProductName = "";
            ProductUnit = "";
            ProductCategory = null;
        }

    }
}
