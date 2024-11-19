﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IT008_QuanLyBanHang.ViewModel
{
    public class RESTService
    {
        public async Task<string> GetAsync(string dataType)
        {
            try
            {
                HttpRequestMessage request = new(HttpMethod.Get, dataType);
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return string.Empty;
            //return await client.SendAsync(request).Result.Content.ReadAsStringAsync();
        }

        #region Singleton
        private static RESTService? instance;
        public static RESTService Instance
        {
            get
            {
                instance ??= new RESTService();
                return instance;
            }
        }
        #endregion

        private RESTService()
        {
            client.BaseAddress = new Uri("https://store-manager-server-8dd2e1e19873.herokuapp.com/api/v1/");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {API.API_TOKEN}");
        }


        HttpClient client = new();
    }
}
