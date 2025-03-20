using System.Net.Http;
using System.Net.Http.Json;

namespace WpfApp.Api
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7136/api/");
        }

        public async Task<Result<List<Doctor>>> List()
        {
            var result = new Result<List<Doctor>>();

            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<List<Doctor>>("Doctors");
            }
            catch(Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }

        public async Task Save(Doctor list)
        {
            if(list.Id == 0)
            {
                await _httpClient.PostAsJsonAsync("Doctors", list);
            }
            else
            {
                await _httpClient.PutAsJsonAsync("Doctors/" + list.Id, list);
            }
        }

        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync("Doctors/" + id);
        }
    }
}