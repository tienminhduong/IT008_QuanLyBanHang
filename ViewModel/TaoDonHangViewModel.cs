using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.Interfaces;
using IT008_QuanLyBanHang.Model;
using IT008_QuanLyBanHang.ViewModel.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        void SearchCustomer()
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
                MessageBox.Show(PhoneText + " không tồn tại trong hệ thống.\nBạn có muốn thêm khách hàng mới",  "Thông báo",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
            }
        }

        List<BatchProduct> productList = new();
        List<Customer> customerList = new();

        [ObservableProperty] ObservableCollection<BatchProduct> products = new();
        [ObservableProperty] string phoneText = "";
        [ObservableProperty] bool isSelectingCustomer = false;
        [ObservableProperty] Customer? selectingCustomer = null;
    }
}
