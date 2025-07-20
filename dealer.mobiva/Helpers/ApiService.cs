using Newtonsoft.Json;
using Objects.ApiModel;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using dealer.mobiva.Helpers;
using Objects.ViewModel;
using Objects;
using System.Reflection;
using System.Web.Mvc;

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

      
        public async Task<GetDealerByIdParameterResult> GetDealerById(int id)
        {
            var param = new { id };
            return await PostAsync<GetDealerByIdParameterResult>($"{ApiUrl}/api/Dealer/GetDealerById", param);
        }
       

        public async Task<GetProductsParameterResult> GetProducts(int dealerId)
        {
            var param = new { dealerId };
            return await PostAsync<GetProductsParameterResult>($"{ApiUrl}/api/Product/GetProducts", param);
        }
        public async Task<GetDealersByAppUserIdParameterResult> GetDealersByAppUserId(int appUserId)
        {
            var getDealersByAppUserIdParameter = new { appUserId };
            return await PostAsync<GetDealersByAppUserIdParameterResult>($"{ApiUrl}/api/Dealer/GetDealersByAppUserId", getDealersByAppUserIdParameter);
        }

        #region GetProductBrand
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

        #region GetProductModel
        public async Task<GetProductModelsParameterResult> GetProductModels(int productBrandId)
        {
            
            var param = new GetProductModelsParameter { ProductBrandId = productBrandId };
            return await PostAsync<GetProductModelsParameterResult>($"{ApiUrl}/api/ProductModel/GetProductModels", param);
        }
        public async Task<SaveProductModelParameterResult> SaveProductModel(ProductModelViewModel model)
        {
            var param = new SaveProductModelParameter { ProductModel = model };
            return await PostAsync<SaveProductModelParameterResult>($"{ApiUrl}/api/ProductModel/SaveProductModel", param);
        }
        public async Task<GetProductModelByIdParameterResult> GetProductModelById(int id)
        {
            var param = new GetProductBrandByIdParameter { Id = id };
            return await PostAsync<GetProductModelByIdParameterResult>($"{ApiUrl}/api/ProductModel/GetProductModelById", param);
        }
        #endregion


        #region AppUser
        public async Task<GetAppUserByIdParameterResult> GetAppUserById(int id)
        {
            var param = new { id };
            return await PostAsync<GetAppUserByIdParameterResult>($"{ApiUrl}/api/AppUser/GetAppUserById", param);
        }

        public async Task<GetAppUsersByDealerIdParameterResult> GetAppUsersByDealerId(int dealerId)
        {
            var param = new GetAppUsersByDealerIdParameter { DealerId = dealerId };
            return await PostAsync<GetAppUsersByDealerIdParameterResult>($"{ApiUrl}/api/AppUser/GetAppUsersByDealerId", param);
        }

        public async Task<SaveAppUserParameterResult> SaveAppUser(AppUserViewModel model)
        {
            var jsonString = JsonConvert.SerializeObject(model);
            var param = new SaveAppUserParameter { AppUser = model };
            return await PostAsync<SaveAppUserParameterResult>($"{ApiUrl}/api/AppUser/SaveAppUser", param);
        }


        #endregion

        #region AppUserToDos


        public async Task<GetAppUserToDosParameterResult> GetAppUserToDos(int appUserId)
        {
            var param = new GetAppUserToDosParameter { AppUserId = appUserId };
            return await PostAsync<GetAppUserToDosParameterResult>($"{ApiUrl}/api/AppUserToDo/GetAppUserToDos", param);
        }

        public async Task<GetAppUserToDoByIdParameterResult> GetAppUserToDoById(int appUserId)
        {
            var param = new GetAppUserToDoByIdParameter { Id = appUserId };
            return await PostAsync<GetAppUserToDoByIdParameterResult>($"{ApiUrl}/api/AppUserToDo/GetAppUserToDoById", param);
        }


        public async Task<SaveAppUserToDoParameterResult> SaveAppUserToDo(AppUserToDoViewModel model)
        {
            var param = new SaveAppUserToDoParameter { AppUserToDo = model };
            return await PostAsync<SaveAppUserToDoParameterResult>($"{ApiUrl}/api/AppUserToDo/SaveAppUserToDo", param);
        }


        #endregion

        public async Task<GetAppUserTypesParameterResult> GetAppUserTypes()
        {
            return await PostAsync<GetAppUserTypesParameterResult>($"{ApiUrl}/api/AppUserType/GetAppUserTypes", null);
        }

        #region Customer

        public async Task<GetCustomerByIdParameterResult> GetCustomerById(int id)
        {
            var param = new { id };
            return await PostAsync<GetCustomerByIdParameterResult>($"{ApiUrl}/api/Customer/GetCustomerById", param);
        }

        public async Task<GetCustomersByDealerIdParameterResult> GetCustomersByDealerId(int dealerId)
        {
            var param = new GetCustomersByDealerIdParameter { DealerId = dealerId };
            return await PostAsync<GetCustomersByDealerIdParameterResult>($"{ApiUrl}/api/Customer/GetCustomersByDealerId", param);
        }

        public async Task<SaveCustomerParameterResult> SaveCustomer(CustomerViewModel model)
        {
            var param = new SaveCustomerParameter { Customer = model };
            return await PostAsync<SaveCustomerParameterResult>($"{ApiUrl}/api/Customer/SaveCustomer", param);
        }

        #endregion

    }
}
