using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Objects.ViewModel;
using Objects;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Objects.ApiModel;
using Newtonsoft.Json;

namespace api.mobiva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public ProductController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetProducts
        [HttpPost("GetProducts")]
        public async Task<GetProductsParameterResult> GetProducts([FromBody] GetProductsParameter body)
        {
            var result = new GetProductsParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<Product>(p => p.ActiveFlg && p.DealerId == body.DealerId);
                result.Products = ObjectHelper.MapList<Product, ProductViewModel>(filtered);
                result.Result = true;
                result.Message = "Success";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
            }

            return result;
        }


        #endregion

        #region GetProductById
        [HttpPost("GetProductById")]
        public async Task<GetProductByIdParameterResult> GetProductById([FromBody] GetProductByIdParameter body)
        {
            var result = new GetProductByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<Product>(body.Id);

                if (entity != null && entity.ActiveFlg)
                {
                    result.Product = ObjectHelper.Map<Product, ProductViewModel>(entity);
                    var entityProduct = await _helper.GetSingleAsync<ProductDetail>(
                    x => x.ProductId == body.Id);

                    result.Product.ProductDetail = ObjectHelper.Map<ProductDetail, ProductDetailViewModel>(entityProduct);
                    result.Result = true;
                    result.Message = "Success";
                }


                else
                {
                    result.Result = false;
                    result.Message = "Product not found or inactive.";
                }
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
            }

            return result;
        }


        #endregion

        #region SaveProduct
        [HttpPost("SaveProduct")]
        public async Task<SaveProductParameterResult> SaveProduct([FromBody] SaveProductParameter body)
        {
            string json = JsonConvert.SerializeObject(body);

            var result = new SaveProductParameterResult();

            try
            {
                var entity = ObjectHelper.Map<ProductViewModel, Product>(body.Product);
                var opResult = await _helper.SaveAsync(entity);

                if (opResult.Success)
                {
                    if (body.Product.ProductDetail == null)
                    {
                        body.Product.ProductDetail = new ProductDetailViewModel();
                    }

                    body.Product.ProductDetail.ProductId = opResult.Data;
                    var entity2 = ObjectHelper.Map<ProductDetailViewModel, ProductDetail>(body.Product.ProductDetail);
                    var opResult2 = await _helper.SaveAsync(entity2);
                }


                result.Result = opResult.Success;
                result.Message = opResult.Message;
                result.Id = opResult.Data;
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
            }

            return result;
        }

        #endregion

        #region GetProductsInventoryByDealerId
        [HttpPost("GetProductsInventoryByDealerId")]
        public async Task<GetProductsInventoryByDealerIdParameterResult> GetProductsInventoryByDealerId([FromBody] GetProductsInventoryByDealerIdParameter body)
        {
            var result = new GetProductsInventoryByDealerIdParameterResult();

            try
            {
                var parameters = new Dictionary<string, object>{
                    { "@DealerId", body.DealerId }};

                result.GetProductsSummaryByDealerId = await _helper.ExecuteStoredProcedureAsync<GetProductsSummaryByDealerId_Result>("GetProductsSummaryByDealerId", parameters);
                result.GetProductsSummaryDetailByDealerId = await _helper.ExecuteStoredProcedureAsync<GetProductsSummaryDetailByDealerId_Result>("GetProductsSummaryDetailByDealerId", parameters);
                result.Result = true;
                result.Message = "Success";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
            }

            return result;
        }


        #endregion
    }
}
