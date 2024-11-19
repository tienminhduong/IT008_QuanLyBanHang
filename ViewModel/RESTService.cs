using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
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
                Trace.WriteLine(ex.Message);
            }
            return string.Empty;
        }

        public async Task<bool> TryLogin(string account, string password)
        {
            string jsonContent = $"{{\"account\":\"{account}\",\"password\":\"{password}\"}}";
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "users/sign_in");
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var respond = await client.SendAsync(request);
                respond.EnsureSuccessStatusCode();
                AccessToken? accessToken = await respond.Content.ReadFromJsonAsync<AccessToken>();
                string? token = accessToken?.data.Token;
                if (!hasLogin)
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                    hasLogin = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return false;
            }
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
        }


        HttpClient client = new();
        bool hasLogin = false;
    }

    class AccessToken
    {
        public AccessTokenData data { get; set; } = new AccessTokenData();
    }
    class AccessTokenData
    {
        [JsonPropertyName("access_token")]
        public string Token { get; set; } = "";
    }
}
