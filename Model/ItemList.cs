using CommunityToolkit.Mvvm.ComponentModel;
using IT008_QuanLyBanHang.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.Model
{
    public class ItemList
    {
        public ItemList()
        {
            Task.Run(() => LoadData());
        }

        async Task LoadData()
        {
            string temp = await RESTService.Instance.GetAsync("products");
            Trace.WriteLine(temp);
            ProductResponse? productResponse = JsonSerializer.Deserialize<ProductResponse>(temp);
            if (productResponse?.Data?.Items != null)
                products = productResponse.Data.Items;

            if (products != null)
                foreach (Product p in products)
                {
                    if (p.Batches != null)
                    {
                        foreach (var b in p.Batches)
                        {
                            BatchProduct bp = new(p, b);
                            BatchProducts ??= new();
                            BatchProducts.Add(bp);
                        }
                    }
                }
        }

        public ObservableCollection<BatchProduct> BatchProducts { get; set; }
        private List<Product> products = new();
    }
}
