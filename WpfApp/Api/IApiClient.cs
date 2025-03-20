namespace WpfApp.Api
{
    public interface IApiClient
    {
        Task<Result<List<Doctor>>> List();
        Task Save(Doctor list);
        Task Delete(int id);
    }
}