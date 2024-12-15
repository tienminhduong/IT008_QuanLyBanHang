using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.DTOs;
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
using System.Windows.Controls;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class KhachHangViewModel : ObservableObject, ITabViewModel
    {
        public KhachHangViewModel()
        {
            Task.Run(() => LoadData());
        }

        [ObservableProperty]
        ObservableCollection<Customer> customers = new();

        List<Customer> customersList = new();
        List<Order> ordersList = new();

        [ObservableProperty] string searchText = "";
        [ObservableProperty] DataGridRowDetailsVisibilityMode showDetails = DataGridRowDetailsVisibilityMode.Collapsed;
        [ObservableProperty] Customer? selectedCustomer = null;

        public async Task LoadData()
        {
            customersList = await CustomerAPI.GetAllCustomers();
            customersList.RemoveAll(c => (c.Phone == "0" || c.Phone == ""));
            ordersList = await OrderAPI.GetAllOrders();

            foreach (Customer customer in customersList)
                customer.UpdateOrderList(ordersList);

            Customers = new ObservableCollection<Customer>(customersList);
        }

        [RelayCommand]
        void Search()
        {
            if (SearchText == "")
                Customers = new(customersList);
            else
                Customers = new(customersList.Where(c => FunctionTool.CheckContains(c.FullName ?? "", SearchText)));
        }

        [RelayCommand]
        async Task Delete()
        {
            if (SelectedCustomer != null)
            {
                try
                {
                    await RESTService.Instance.DeleteAsync($"customers/{SelectedCustomer.Id}");
                    await LoadData();
                }
                catch (Exception)
                {
                    MessageBox.Show("Không thể xóa khách đã mua hàng", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
