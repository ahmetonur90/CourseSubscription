using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace Subscription.API.Helpers
{
    public class ObjectActionResult : IActionResult
    {
        public bool Success { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public MsgObject Message { get; private set; }
        public dynamic Data { get; private set; }

        public class MsgObject
        {
            public string Statu { get; set; }
            public string Header { get; set; }
            public dynamic Content { get; set; }
        }
        public ObjectActionResult(bool success, HttpStatusCode statusCode, MsgObject message, dynamic data)
        {
            this.Success = success;
            this.StatusCode = statusCode;
            this.Message = message;
            this.Data = data;
        }

        public ObjectActionResult(bool success, HttpStatusCode statusCode, MsgObject message)
        {
            this.Success = success;
            this.StatusCode = statusCode;
            this.Message = message;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(
                new
                {
                    this.Success,
                    this.Message,
                    this.Data
                })
            {
                StatusCode = (int)StatusCode
            };

            await objectResult.ExecuteResultAsync(context);
        }

        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

}
