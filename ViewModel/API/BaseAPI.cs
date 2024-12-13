using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Humanizer;

namespace IT008_QuanLyBanHang.ViewModel.API
{
    public class BaseAPI<T, DTO>
        where T: class
        where DTO : class
    {
        public static async Task<List<DTO>> GetAllItemsDTO()
        {
            Trace.WriteLine(typeof(T).Name.Pluralize().ToLower());
            string response = await RESTService.Instance.GetAsyncOfType(typeof(T).Name.Pluralize().ToLower());
            Trace.WriteLine($"API response in BaseAPI for the type of {typeof(T).Name}: {response}");
            APIResponse? res;
            try
            {
                res = JsonSerializer.Deserialize<APIResponse>(response);
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Error in BaseAPI for the type of {typeof(T).Name}: {e.Message}");
                return new();
            }
            if (res == null || res.data == null || res.data.items == null)
                return new();

            Trace.WriteLine($"Count items return by {typeof(T).Name}API: {res.data.items.Count}");
            return res.data.items;
        }

        class Data
        {
            public List<DTO>? items { get; set; }
        }
        class APIResponse
        {
            public Data? data { get; set; }
        }
    }
}
