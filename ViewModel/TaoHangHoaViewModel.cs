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

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class TaoHangHoaViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? productName;

        [ObservableProperty]
        private string? productUnit;

        [ObservableProperty]
        private string? productCategory;

        [ObservableProperty]
        private string? productExpiration;

        [ObservableProperty]
        private ObservableCollection<int>? categoriesIdList = new ObservableCollection<int>();

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
            string jsonResponse = await RESTService.Instance.GetAsync("categories");
            Trace.WriteLine($"Raw JSON Response: {jsonResponse}");

            if (!string.IsNullOrEmpty(jsonResponse))
            {
                try
                {
                    // Deserialize the JSON into the wrapper class
                    var categoryResponse = JsonSerializer.Deserialize<CategoryResponse>(jsonResponse);
                    Trace.WriteLine($"Deserialized Data: {categoryResponse?.Data?.Items?.Count}");

                    // Extract the list of categories from the nested "items"
                    if (categoryResponse?.Data?.Items != null)
                    {
                        Trace.WriteLine("Populating CategoriesList");
                        ObservableCollection<Category> categoriesList = new ObservableCollection<Category>(categoryResponse.Data.Items);

                        foreach (var category in categoriesList)
                        {
                            if (category != null)
                            {
                                CategoriesIdList?.Add(category.Id);
                            }    
                        }    
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Error deserializing categories: {ex.Message}");
                }
            }
        }



        [RelayCommand]
        private async Task AddProduct()
        {
            if (string.IsNullOrWhiteSpace(ProductName) || string.IsNullOrWhiteSpace(ProductCategory))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var productData = new Dictionary<string, string>
            {
                { "product_name", ProductName },
                { "category_id", ProductCategory },
                { "status", "active" }
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

    }
}
