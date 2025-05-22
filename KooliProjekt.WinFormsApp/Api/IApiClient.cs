namespace KooliProjekt.WinFormsApp.Api

{

    public interface IApiClient

    {

        Task<Result<List<Doctor>>> List();

        Task<Result> Save(Doctor doctor);

        Task<Result> Delete(int id);

    }

}
