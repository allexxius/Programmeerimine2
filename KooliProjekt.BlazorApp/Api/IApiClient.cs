namespace KooliProjekt.BlazorApp
{
    public interface IApiClient
    {
        Task<Result<Doctor>> Get(int id);
        Task<Result<List<Doctor>>> List();
        Task<Result> Save(Doctor list);
        Task Delete(int id);
    }
}