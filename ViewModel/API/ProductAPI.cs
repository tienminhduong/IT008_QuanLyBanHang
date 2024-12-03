using IT008_QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.ViewModel.API
{
    public class ProductAPI
    {
        public ProductAPI()
        {
            string temp = Task.Run(() => RESTService.Instance.GetAsyncOfType("products")).Result;
            response = JsonSerializer.Deserialize<Response>(temp);
            Trace.WriteLine(temp);
        }

        Response? response;
        public List<Product>? Products => response?.Data?.Items;

        public class Response
        {
            [JsonPropertyName("data")]
            public Data? Data { get; set; }
        }

        public class Data
        {
            [JsonPropertyName("items")]
            public List<Product>? Items { get; set; }
        }
    }
}
