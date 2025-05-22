using System.Threading.Tasks;

namespace KooliProjekt.WinFormsApp
{
    public interface IDoctorPresenter
    {
        Task Initialize();
        Task DeleteDoctor();
        Task SaveDoctor();
        void NewDoctor();
        void DoctorSelected();
    }
}