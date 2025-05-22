// ApiClient.cs
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
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }

        public async Task<Result> Save(Doctor doctor)
        {
            var result = new Result();

            try
            {
                if (doctor.Id == 0)
                {
                    var response = await _httpClient.PostAsJsonAsync("Doctors", doctor);
                    if (!response.IsSuccessStatusCode)
                    {
                        result.Error = await response.Content.ReadAsStringAsync();
                    }
                }
                else
                {
                    var response = await _httpClient.PutAsJsonAsync("Doctors/" + doctor.Id, doctor);
                    if (!response.IsSuccessStatusCode)
                    {
                        result.Error = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }

        public async Task<Result> Delete(int id)
        {
            var result = new Result();

            try
            {
                var response = await _httpClient.DeleteAsync("Doctors/" + id);
                if (!response.IsSuccessStatusCode)
                {
                    result.Error = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }
    }
}