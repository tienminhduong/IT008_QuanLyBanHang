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
    public static class BatchAPI
    {
        static public async Task<List<Batch>?> GetAllBatches()
        {
            string resStr = await RESTService.Instance.GetAsyncOfType("batches");
            Response? res = JsonSerializer.Deserialize<Response>(resStr);
            if (res == null || res.Data == null)
                return null;
            return res.Data.Items;
        }

        class Data
        {
            [JsonPropertyName("items")]
            public List<Batch>? Items { get; set; }
        }
        class Response
        {
            [JsonPropertyName("data")]
            public Data? Data { get; set; }
        }
    }
}
