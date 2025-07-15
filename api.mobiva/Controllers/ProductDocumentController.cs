using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Objects.ViewModel;
using Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Objects.ApiModel;

namespace api.mobiva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDocumentController : ControllerBase
    {
        private readonly ContextApi _context;
        private readonly ContextApiHelper _helper;

        public ProductDocumentController(ContextApi context)
        {
            _context = context;
            _helper = new ContextApiHelper(_context);
        }

        #region GetProductDocuments
        [HttpPost("GetProductDocuments")]
        public async Task<GetProductDocumentsParameterResult> GetProductDocuments([FromBody] GetProductDocumentsParameter body)
        {
            var result = new GetProductDocumentsParameterResult();

            try
            {
                var filtered = await _helper.GetAllAsync<ProductDocument>(pd => pd.ActiveFlg && pd.ProductId == body.ProductId);
                result.ProductDocuments = ObjectHelper.MapList<ProductDocument, ProductDocumentViewModel>(filtered);
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

        #region GetProductDocumentById
        [HttpPost("GetProductDocumentById")]
        public async Task<GetProductDocumentByIdParameterResult> GetProductDocumentById([FromBody] GetProductDocumentByIdParameter body)
        {
            var result = new GetProductDocumentByIdParameterResult();

            try
            {
                var entity = await _helper.GetByIdAsync<ProductDocument>(body.ProductDocumentId);

                if (entity != null && entity.ActiveFlg)
                {
                    result.ProductDocument = ObjectHelper.Map<ProductDocument, ProductDocumentViewModel>(entity);
                    result.Result = true;
                    result.Message = "Success";
                }
                else
                {
                    result.Result = false;
                    result.Message = "ProductDocument not found or inactive.";
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

        #region SaveProductDocument
        [HttpPost("SaveProductDocument")]
        public async Task<SaveProductDocumentParameterResult> SaveProductDocument([FromForm] SaveProductDocumentFormParameter body)
        {
            var result = new SaveProductDocumentParameterResult();

            try
            {
                if (body.File == null || body.File.Length == 0)
                {
                    result.Result = false;
                    result.Message = "Dosya yüklenmedi.";
                    return result;
                }

                var extension = System.IO.Path.GetExtension(body.File.FileName);
                var newFileName = $"{Guid.NewGuid()}{extension}";
                var savePath = System.IO.Path.Combine("wwwroot/images/products", newFileName);

                using (var stream = System.IO.File.Create(savePath))
                {
                    await body.File.CopyToAsync(stream);
                }

                var entity = new ProductDocument
                {
                    Id = body.Id,
                    ProductId = body.ProductId,
                    ProductDocumentTypeId = body.ProductDocumentTypeId,
                    UId = Guid.NewGuid().ToString(),
                    FileName = body.File.FileName,
                    FileExtension = extension,
                    Detail = body.Detail,
                    CreateUserId = body.CreateUserId, // Burada client'tan alınıyor
                    CreateDate = DateTime.UtcNow,
                    ActiveFlg = true
                };

                var opResult = await _helper.SaveAsync(entity);

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
    }
}
