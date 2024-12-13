using IT008_QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.ViewModel.API
{
    public static class CustomerAPI
    {
        static public async Task<List<Customer>?> GetAllCustomer()
        {
            string resStr = await RESTService.Instance.GetAsyncOfType("customers");
            Response? res = JsonSerializer.Deserialize<Response>(resStr);
            if (res == null || res.Data == null)
                return null;

            return res.Data.Items;
        }

        class Data
        {
            [JsonPropertyName("items")]
            public List<Customer>? Items { get; set; }
        }
        class Response
        {
            [JsonPropertyName("data")]
            public Data? Data { get; set; }
        }
    }
}
