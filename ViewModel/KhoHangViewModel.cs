using CommunityToolkit.Mvvm.ComponentModel;
using IT008_QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class KhoHangViewModel : ObservableObject
    {
        public KhoHangViewModel()
        {
            batchProducts = [];
            products = [];
            Task.Run(() => LoadData());
        }

        async Task LoadData()
        {
            string temp = await RESTService.Instance.GetAsync("products");
            ProductResponse? productResponse = JsonSerializer.Deserialize<ProductResponse>(temp);
            if (productResponse?.Data?.Items != null)
                products = productResponse.Data.Items;

            if (products == null)
                return;

            foreach (Product p in products)
            {
                if (p.Batches != null)
                {
                    foreach (var b in p.Batches)
                    {
                        BatchProduct bp = new(p, b);
                        BatchProducts.Add(bp);
                    }
                }
            }

            foreach (var p in products)
            {
                Trace.WriteLine(p.ProductName);
            }
        }

        List<Product>? products;

        [ObservableProperty]
        List<BatchProduct> batchProducts;
    }
}
