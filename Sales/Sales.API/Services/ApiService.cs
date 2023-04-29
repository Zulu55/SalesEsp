using Newtonsoft.Json;
using Sales.Shared.Responses;

namespace Sales.API.Services
{
    public class ApiService : IApiService
    {
        private readonly string _urlBase;
        private readonly string _tokenName;
        private readonly string _tokenValue;

        public ApiService(IConfiguration configuration)
        {
            _urlBase = configuration["CoutriesAPI:urlBase"]!;
            _tokenName = configuration["CoutriesAPI:tokenName"]!;
            _tokenValue = configuration["CoutriesAPI:tokenValue"]!;
        }

        public async Task<Response<List<T>>> GetListAsync<T>(string servicePrefix, string controller)
        {
            try
            {
                var client = new HttpClient()
                {
                    BaseAddress = new Uri(_urlBase),
                };

                client.DefaultRequestHeaders.Add(_tokenName, _tokenValue);
                var url = $"{servicePrefix}{controller}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response<List<T>>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                List<T> list = JsonConvert.DeserializeObject<List<T>>(result)!;
                return new Response<List<T>>
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response<List<T>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
