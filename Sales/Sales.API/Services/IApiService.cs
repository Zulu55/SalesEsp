using Sales.Shared.Responses;

namespace Sales.API.Services
{
    public interface IApiService
    {
        Task<Response<List<T>>> GetListAsync<T>(string servicePrefix, string controller);
    }
}
