using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class TongQuanViewModel : ObservableObject
    {
        public TongQuanViewModel()
        {
            Task.Run(() => LoadData());
        }

        [ObservableProperty]
        List<Product>? products = null;

        async Task LoadData()
        {
            string temp = await RESTService.Instance.GetAsync("products");
            ProductResponse? productResponse = JsonSerializer.Deserialize<ProductResponse>(temp);
            if (productResponse?.Data?.Items != null)
                Products = productResponse.Data.Items;
        }
    }
}
