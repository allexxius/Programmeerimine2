using System;

using System.Collections.Generic;

using System.Net.Http;

using System.Net.Http.Json;

using System.Threading.Tasks;

namespace WpfApp.Api

{

    public class ApiClient : IApiClient

    {

        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)

        {

            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri("https://localhost:7136/api/");

        }

        public async Task<Result<List<Doctor>>> List()

        {

            try

            {

                var response = await _httpClient.GetAsync("Doctors");

                if (!response.IsSuccessStatusCode)

                {

                    return Result<List<Doctor>>.Fail(await response.Content.ReadAsStringAsync());

                }

                var data = await response.Content.ReadFromJsonAsync<List<Doctor>>();

                return Result<List<Doctor>>.Success(data);

            }

            catch (Exception ex)

            {

                return Result<List<Doctor>>.Fail($"Failed to load doctors: {ex.Message}");

            }

        }

        public async Task<Result> Save(Doctor doctor)

        {

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

                    return Result.Fail(await response.Content.ReadAsStringAsync());

                }

                return Result.Success();

            }

            catch (Exception ex)

            {

                return Result.Fail($"Failed to save doctor: {ex.Message}");

            }

        }

        public async Task<Result> Delete(int id)

        {

            try

            {

                var response = await _httpClient.DeleteAsync($"Doctors/{id}");

                if (!response.IsSuccessStatusCode)

                {

                    return Result.Fail(await response.Content.ReadAsStringAsync());

                }

                return Result.Success();

            }

            catch (Exception ex)

            {

                return Result.Fail($"Failed to delete doctor: {ex.Message}");

            }

        }

    }

}
