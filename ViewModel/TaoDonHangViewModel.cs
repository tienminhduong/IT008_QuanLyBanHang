using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.Interfaces;
using IT008_QuanLyBanHang.Model;
using IT008_QuanLyBanHang.View;
using IT008_QuanLyBanHang.ViewModel.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class TaoDonHangViewModel : ObservableObject, ITabViewModel
    {
        public TaoDonHangViewModel()
        {
            Task.Run(() => LoadData());
        }

        public async Task LoadData()
        {
            productList = await ProductAPI.GetAllProductsWithBatches();
            Products = new(productList);

            customerList = await CustomerAPI.GetAllCustomers();
        }

        [RelayCommand]
        async Task SearchCustomer()
        {
            if (PhoneText == "")
                return;

            // If PhoneText have any character other than number, return
            if (PhoneText.Any(c => !char.IsDigit(c)))
                return;

            SelectingCustomer = customerList.Find(c => c.Phone == PhoneText);
            if (SelectingCustomer != null)
            {
                // Set selectingCustomer to the selectingCustomer found
                PhoneText = SelectingCustomer?.FullName ?? "";
                IsSelectingCustomer = true;
            }
            else
            {
                var result = MessageBox.Show(PhoneText + " không tồn tại trong hệ thống.\nBạn có muốn thêm khách hàng mới",  "Thông báo",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    string name = Microsoft.VisualBasic.Interaction.InputBox("Nhập họ và tên của khách hàng", "Thêm khách hàng mới");
                    // detach name into first name and last name
                    string[] names = name.Split(' ');
                    if (names.Length < 2)
                    {
                        MessageBox.Show("Họ và tên không hợp lệ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    KhachHangDTO newCustomer = new()
                    {
                        first_name = names[^1],
                        last_name = string.Join(' ', names[..^1]),
                        phone = PhoneText,
                        status = 1,
                        birth = DateTime.Now
                    };
                    string body = JsonSerializer.Serialize(newCustomer);
                    //post to server
                    string response = await RESTService.Instance.PostAsync("customers", body);
                    customerList = await CustomerAPI.GetAllCustomers();
                }
            }
        }

        [RelayCommand]
        void AddToCart(string number)
        {
            int quantity;
            if (!int.TryParse(number, out quantity))
                return;

            if (quantity <= 0)
                return;

            if (SelectedProduct == null)
                return;

            int currentQuantity = 0;
            if (Cart.Contains(SelectedProduct))
                currentQuantity = Cart.First(p => p == SelectedProduct).SelectedQuantity;

            if (currentQuantity + quantity > SelectedProduct.Batch.Stock)
            {
                MessageBox.Show("Số lượng hàng trong kho không đủ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Cart.Contains(SelectedProduct))
            {
                SelectedProduct.SelectedQuantity = quantity;
                Cart.Add(SelectedProduct);
            }
            else
                Cart.First(p => p == SelectedProduct).SelectedQuantity += quantity;

            TotalPrice += SelectedProduct.Batch.Price * quantity;
        }

        [RelayCommand]
        void RemoveFromCart()
        {
            if (SelectedCartProduct == null)
                return;
            TotalPrice -= SelectedCartProduct.Batch.Price * SelectedCartProduct.SelectedQuantity;
            SelectedCartProduct.SelectedQuantity = 0;
            Cart.Remove(SelectedCartProduct);
        }

        [RelayCommand]
        async Task Check(Window window)
        {
            if (SelectingCustomer == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Cart.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Create new order
            DonHangDTO order = new()
            {
                customer_id = SelectingCustomer.Id,
                status = "completed",
                discount = 0,
                items = new()
            };

            foreach (var product in Cart)
            {
                order.items.Add(new()
                {
                    batch_id = product.Batch.Id,
                    quantity = product.SelectedQuantity
                });
            }

            // Send order to server
            string body = JsonSerializer.Serialize(order);
            Trace.WriteLine(body);

            string response = await RESTService.Instance.PostAsync("orders", body);
            Trace.WriteLine(response);
            window.Close();
        }

        List<BatchProduct> productList = new();
        List<Customer> customerList = new();

        [ObservableProperty] ObservableCollection<BatchProduct> products = new();
        [ObservableProperty] string phoneText = "";
        [ObservableProperty] bool isSelectingCustomer = false;
        [ObservableProperty] Customer? selectingCustomer = null;
        [ObservableProperty] BatchProduct? selectedProduct = null;
        [ObservableProperty] string selectedNumber = "";
        [ObservableProperty] ObservableCollection<BatchProduct> cart = new();
        [ObservableProperty] BatchProduct? selectedCartProduct = null;
        [ObservableProperty] float totalPrice = 0;
    }

    public class PriceCalculatorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is float price && values[1] is int quantity)
            {
                Trace.WriteLine($"Price: {price}, Quantity: {quantity}");
                return $"{(price * quantity)}đ";
            }
            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return [];
        }
    }

    public class DonHangDTO
    {
        public int customer_id { get; set; }
        public string? status { get; set; }
        public int discount { get; set; }
        public List<DonHangItemDTO>? items { get; set; }
        public class DonHangItemDTO
        {
            public int batch_id { get; set; }
            public int quantity { get; set; }
        }

        List<BatchProduct> productList = new();
        List<Customer> customerList = new();
    }

    public class KhachHangDTO
    {
        public string password { get; set; } = "string";
        public string first_name { get; set; } = "";
        public string last_name { get; set; } = "";
        public string phone { get; set; } = "";
        public int status { get; set; }
        public DateTime birth { get; set; }
    }
}
