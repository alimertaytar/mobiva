using System;

namespace Objects.ApiModel
{
    public class BaseParameterResult
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public string InternalMessage { get; set; } // Teknik detaylar için
        public object Data { get; set; }
        public int Id { get; set; }

        public static BaseParameterResult Success(object data = null, string message = "İşlem başarılı.")
        {
            return new BaseParameterResult
            {
                Result = true,
                Message = message,
                Data = data
            };
        }

        public static BaseParameterResult Fail(string message, Exception ex = null)
        {
            return new BaseParameterResult
            {
                Result = false,
                Message = message,
                InternalMessage = ex?.ToString()
            };
        }
    }
}
