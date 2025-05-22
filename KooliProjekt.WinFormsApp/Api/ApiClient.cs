using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KooliProjekt.WinFormsApp.Api
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
            catch (HttpRequestException ex)
            {
                result.Error = ex.StatusCode == null
                    ? "Cannot connect to server. Please try again later."
                    : ex.Message;
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
                HttpResponseMessage response;

                if (doctor.Id == 0)
                {
                    response = await _httpClient.PostAsJsonAsync("Doctors", doctor);
                }
                else
                {
                    response = await _httpClient.PutAsJsonAsync($"Doctors/{doctor.Id}", doctor);
                }

                if (!response.IsSuccessStatusCode)
                {
                    result.Error = await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                result.Error = ex.StatusCode == null
                    ? "Cannot connect to server. Please try again later."
                    : ex.Message;
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
                var response = await _httpClient.DeleteAsync($"Doctors/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    result.Error = await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                result.Error = ex.StatusCode == null
                    ? "Cannot connect to server. Please try again later."
                    : ex.Message;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }
    }
}