using CommunityToolkit.Mvvm.ComponentModel;
using IT008_QuanLyBanHang.Interfaces;
using IT008_QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task LoadData()
        {
            List<Customer>? customerList = await API.CustomerAPI.GetAllCustomer();
            if (customerList == null)
                return;
            Customers = new ObservableCollection<Customer>(customerList);
        }
    }
}
