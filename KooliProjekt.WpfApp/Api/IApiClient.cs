namespace KooliProjekt.WpfApp.Api
{
    public interface IApiClient
    {
        Task<Result<List<TodoList>>> List();
        Task Save(TodoList list);
        Task Delete(int id);
    }
}