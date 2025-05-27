using System;

using System.Collections.Generic;

using System.Net.Http;

using System.Net.Http.Json;

using System.Threading.Tasks;

namespace KooliProjekt.BlazorApp

{

    public class ApiClient : IApiClient

    {

        private readonly HttpClient _httpClient;

        // Konstruktor, mis kasutab DI kaudu antud HttpClienti

        public ApiClient(HttpClient httpClient)

        {

            _httpClient = httpClient;

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

                result.AddError("_", ex.Message);

            }

            return result;

        }

        public async Task<Result> Save(Doctor list)

        {

            HttpResponseMessage response;

            if (list.Id == 0)

            {

                response = await _httpClient.PostAsJsonAsync("Doctors", list);

            }

            else

            {

                response = await _httpClient.PutAsJsonAsync("Doctors/" + list.Id, list);

            }

            if (!response.IsSuccessStatusCode)

            {

                var result = await response.Content.ReadFromJsonAsync<Result>();

                return result;

            }

            return new Result();

        }

        public async Task Delete(int id)

        {

            await _httpClient.DeleteAsync("Doctors/" + id);

        }

        public async Task<Result<Doctor>> Get(int id)

        {

            var result = new Result<Doctor>();

            try

            {

                result.Value = await _httpClient.GetFromJsonAsync<Doctor>("Doctors/" + id);

            }

            catch (Exception ex)

            {

                result.AddError("_", ex.Message);

            }

            return result;

        }

    }

}

