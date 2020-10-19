using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Hydra.Core.Communication;

namespace Hydra.Bff.Orders.Services
{
    public abstract class Service
    {
        protected StringContent GetContent(object data) =>
            new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json"
            );
        
        protected async Task<T> DeserializeResponseObject<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        protected bool CheckResponseError(HttpResponseMessage responseMessage)
        {
            if(responseMessage.StatusCode == HttpStatusCode.BadRequest) return false;

            responseMessage.EnsureSuccessStatusCode();
            return true;
        }

        protected ResponseResult ReturnOk()
        {
            return new ResponseResult();
        }
    }
}