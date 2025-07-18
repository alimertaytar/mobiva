﻿using Newtonsoft.Json;
using Objects.ApiModel;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using dealer.mobiva.Helpers;
using Objects.ViewModel;

namespace dealer.mobiva.Helpers
{
    public class ApiService
    {
        private static readonly HttpClient _client = new HttpClient();
        private const string ApiUrl = "http://localhost"; // Prod ortamda değiştir

        public ApiService()
        {
            // Constructor boş bırakılabilir
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var token = SessionManager.AccessToken;

            if (!string.IsNullOrEmpty(token))
                return token;

            token = CookieManager.CookieGet("AccessToken");
            if (!string.IsNullOrEmpty(token))
            {
                SessionManager.AccessToken = token;
                return token;
            }

            // Token yoksa API'den al
            var authRequest = new
            {
                username = "ama",
                password = "ama"
            };

            var content = new StringContent(JsonConvert.SerializeObject(authRequest), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{ApiUrl}/api/Authorize", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseBody);

            token = result.accessToken;
            if (string.IsNullOrEmpty(token))
                return null;

            SessionManager.AccessToken = token;
            CookieManager.CookieCreate("AccessToken", token);

            return token;
        }

        private async Task PrepareAuthorizationAsync()
        {
            var token = await GetAccessTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                // Her istekte header'ı güncelle
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _client.DefaultRequestHeaders.Authorization = null;
            }
        }

        public async Task<T> PostAsync<T>(string url, object data)
        {
            await PrepareAuthorizationAsync();

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);

            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"API POST Error: {response.StatusCode} - {responseBody}");

            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public async Task<LoginParameterResult> LoginAsync(string email, string password)
        {
            var param = new { email, password };
            return await PostAsync<LoginParameterResult>($"{ApiUrl}/api/Login/Login", param);
        }

        public async Task<GetAppUserLoginParameterResult> GetAppUserLogin(string email, string password)
        {
            var param = new { email, password };
            return await PostAsync<GetAppUserLoginParameterResult>($"{ApiUrl}/api/AppUser/GetAppUserLogin", param);
        }

        public async Task<GetAppUserByIdParameterResult> GetAppUserById(int id)
        {
            var param = new { id };
            return await PostAsync<GetAppUserByIdParameterResult>($"{ApiUrl}/api/AppUser/GetAppUserById", param);
        }
        public async Task<GetDealerByIdParameterResult> GetDealerById(int id)
        {
            var param = new { id };
            return await PostAsync<GetDealerByIdParameterResult>($"{ApiUrl}/api/Dealer/GetDealerById", param);
        }
        public async Task<GetDealersByAppUserIdParameterResult> GetDealersByAppUserId(int appUserId)
        {
            var getDealersByAppUserIdParameter = new { appUserId };
            return await PostAsync<GetDealersByAppUserIdParameterResult>($"{ApiUrl}/api/Dealer/GetDealersByAppUserId", getDealersByAppUserIdParameter);
        }

        public async Task<GetProductsParameterResult> GetProducts(int dealerId)
        {
            var param = new { dealerId };
            return await PostAsync<GetProductsParameterResult>($"{ApiUrl}/api/Product/GetProducts", param);
        }


        #region GetProductBrands
        public async Task<GetProductBrandsParameterResult> GetProductBrands()
        {
            return await PostAsync<GetProductBrandsParameterResult>($"{ApiUrl}/api/ProductBrand/GetProductBrands", null);
        }
        public async Task<SaveProductBrandParameterResult> SaveProductBrand(ProductBrandViewModel model)
        {
            var param = new SaveProductBrandParameter { ProductBrand = model };
            return await PostAsync<SaveProductBrandParameterResult>($"{ApiUrl}/api/ProductBrand/SaveProductBrand", param);
        }
        public async Task<GetProductBrandByIdParameterResult> GetProductBrandById(int id)
        {
            var param = new GetProductBrandByIdParameter { Id = id };
            return await PostAsync<GetProductBrandByIdParameterResult>($"{ApiUrl}/api/ProductBrand/GetProductBrandById", param);
        }
        #endregion
    }
}
