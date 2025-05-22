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

        public ApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7136/api/");
        }

        public async Task<Result<List<TodoList>>> List()
        {
            var result = new Result<List<TodoList>>();

            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<List<TodoList>>("TodoLists");
            }
            catch (Exception ex)
            {
                result.AddError("_", ex.Message);
            }

            return result;
        }

        public async Task<Result> Save(TodoList list)
        {
            HttpResponseMessage response;

            if(list.Id == 0)
            {
                response = await _httpClient.PostAsJsonAsync("TodoLists", list);
            }
            else
            {
                response = await _httpClient.PutAsJsonAsync("TodoLists/" + list.Id, list);
            }

            if(!response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Result>();
                return result;
            }

            return new Result();
        }

        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync("TodoLists/" + id);
        }

        public async Task<Result<TodoList>> Get(int id)
        {
            var result = new Result<TodoList>();

            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<TodoList>("TodoLists/" + id);
            }
            catch (Exception ex)
            {
                result.AddError("_", ex.Message);
            }

            return result;
        }
    }
}