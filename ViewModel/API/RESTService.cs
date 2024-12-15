using IT008_QuanLyBanHang.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace IT008_QuanLyBanHang.ViewModel.API
{
    public class RESTService
    {
        public async Task<string> GetAsyncOfType(string dataType)
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

        public async Task<string> PostAsync(string endpoint, Dictionary<string, string> formData)
        {
            try
            {
                var content = new FormUrlEncodedContent(formData);
                var response = await client.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error during POST to {endpoint}: {ex.Message}");
                return string.Empty;
            }
        }


        public async Task<string> PostAsync(string endpoint, string body)
        {
            try
            {
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error during POST to {endpoint}: {ex.Message}");
            }
            return string.Empty;
        }

        public async Task<string> PutAsync(string endpoint, Dictionary<string, string> formData)
        {
            try
            {
                var content = new FormUrlEncodedContent(formData);
                var response = await client.PutAsync(endpoint, content);
                response.EnsureSuccessStatusCode();


                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

                Trace.WriteLine($"Error during PUT to {endpoint}: {ex.Message}");
                return string.Empty;
            }
        }

        public async Task<string> PutAsync(string endpoint, string body)
        {
            try
            {
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(endpoint, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

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

        public async Task DeleteAsync(string endpoint)
        {
            try
            {
                var response = await client.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> UploadProductImage(string filename, int id)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"products/{id}/upload_image_signature");
            var res = await client.SendAsync(req);
            res.EnsureSuccessStatusCode();
            CloudinaryResponseData? cloudinaryResponseData = await res.Content.ReadFromJsonAsync<CloudinaryResponseData>();
            if (cloudinaryResponseData == null)
                return "";
            Trace.WriteLine(filename);
            using (MultipartFormDataContent content = new())
            using (HttpClient cloudinaryClient = new())
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                var fileContent = new ByteArrayContent(File.ReadAllBytes(filename));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                content.Add(fileContent, "file");
                content.Add(new StringContent(cloudinaryResponseData.data.api_key), "api_key");
                content.Add(new StringContent(cloudinaryResponseData.data.public_id), "public_id");
                content.Add(new StringContent(cloudinaryResponseData.data.timestamp.ToString()), "timestamp");
                content.Add(new StringContent(cloudinaryResponseData.data.signature), "signature");

                Trace.WriteLine("Content: " + content.ToString());
                //print api_key, public_id, timestamp, signature
                Trace.WriteLine(cloudinaryResponseData.data.api_key);
                Trace.WriteLine(cloudinaryResponseData.data.public_id);
                Trace.WriteLine(cloudinaryResponseData.data.timestamp);
                Trace.WriteLine(cloudinaryResponseData.data.signature);
                Trace.WriteLine($"https://api.cloudinary.com/v1_1/{cloudinaryResponseData.data.cloud_name}/image/upload");

                var response = await cloudinaryClient.PostAsync($"https://api.cloudinary.com/v1_1/{cloudinaryResponseData.data.cloud_name}/image/upload", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Trace.WriteLine($"Response Content: {responseContent}");
                response.EnsureSuccessStatusCode();

                ImageUrlFromResponse? imageUrl = await response.Content.ReadFromJsonAsync<ImageUrlFromResponse>();
                return imageUrl?.secure_url ?? "";
            }
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
                string? token = accessToken?.data.access_token;

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

        private RESTService()
        {
            client.BaseAddress = new Uri("https://store-manager-server-8dd2e1e19873.herokuapp.com/api/v1/");
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

        private static readonly HttpClient httpClient = new();
        #endregion

        HttpClient client = httpClient;
        bool hasLogin = false;
    }

    class AccessToken
    {
        public AccessTokenData data { get; set; } = new AccessTokenData();
    }

    class AccessTokenData
    {
        public string access_token { get; set; } = "";
    }
    class CloudinaryResponseData
    {
        public UploadImageResponesDTO data { get; set; }
    }
    class ImageUrlFromResponse
    {
        public string secure_url { get; set; }
    }
}
