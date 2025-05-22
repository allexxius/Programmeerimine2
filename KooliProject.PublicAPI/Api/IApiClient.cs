namespace KooliProjekt.BlazorApp
{
    public interface IApiClient
    {
        Task<Result<TodoList>> Get(int id);
        Task<Result<List<TodoList>>> List();
        Task<Result> Save(TodoList list);
        Task Delete(int id);
    }
}