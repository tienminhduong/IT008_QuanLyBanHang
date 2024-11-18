using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.ViewModel
{
    public class REST_Service
    {
        public async Task<string> GetAsync(string dataType)
        {
            HttpRequestMessage request = new(HttpMethod.Get, dataType);
            HttpResponseMessage response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
            //return await client.SendAsync(request).Result.Content.ReadAsStringAsync();
        }

        #region Singleton
        private static REST_Service? instance;
        public static REST_Service Instance
        {
            get
            {
                instance ??= new REST_Service();
                return instance;
            }
        }
        #endregion

        private REST_Service()
        {
            client.BaseAddress = new Uri("https://store-manager-server-8dd2e1e19873.herokuapp.com/api/v1/");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {API.API_TOKEN}");
        }


        HttpClient client = new();
    }
}
