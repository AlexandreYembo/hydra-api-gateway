using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Hydra.Bff.Orders.Extensions;
using Hydra.Bff.Orders.Models;
using Microsoft.Extensions.Options;

namespace Hydra.Bff.Orders.Services
{
    public interface IVoucherService
    {
        Task<VoucherDTO> GetVoucherByCode(string code);
    }
    
    public class VoucherService : Service, IVoucherService
    {
        private readonly HttpClient _httpClient;

        public VoucherService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri(settings.Value.VoucherUrl);
        }

        public async Task<VoucherDTO> GetVoucherByCode(string code)
        {
            var response = await _httpClient.GetAsync($"/voucher/{code}");

            if(response.StatusCode == HttpStatusCode.NotFound) return null;

            CheckResponseError(response);
            
            return await DeserializeResponseObject<VoucherDTO>(response);
        }
    }
}