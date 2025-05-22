using KooliProjekt.WinFormsApp.Api;

using System.Threading.Tasks;

namespace KooliProjekt.WinFormsApp

{

    public class DoctorPresenter : IDoctorPresenter

    {

        private readonly IApiClient _apiClient;

        private readonly IDoctorView _view;

        public DoctorPresenter(IDoctorView view, IApiClient apiClient)

        {

            _view = view;

            _apiClient = apiClient;

        }

        public async Task Initialize()

        {

            await LoadDoctors();

        }

        private async Task LoadDoctors()

        {

            var response = await _apiClient.List();

            if (response.HasError)

            {

                _view.ShowMessage($"Error loading doctors: {response.Error}", "Error",

                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;

            }

            _view.Doctors = response.Value;

        }

        public async Task DeleteDoctor()

        {

            if (_view.Id == 0)

            {

                _view.ShowMessage("Please select a doctor to delete.", "Error",

                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;

            }

            if (_view.ConfirmDelete("Are you sure you want to delete this doctor?", "Confirm Delete"))

            {

                var result = await _apiClient.Delete(_view.Id);

                if (result.HasError)

                {

                    _view.ShowMessage($"Error deleting doctor: {result.Error}", "Error",

                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                else

                {

                    _view.ShowMessage("Doctor deleted successfully.", "Success",

                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await LoadDoctors();

                    _view.ClearFields();

                }

            }

        }

        public async Task SaveDoctor()

        {

            if (string.IsNullOrWhiteSpace(_view.Name) || string.IsNullOrWhiteSpace(_view.Specialization))

            {

                _view.ShowMessage("Please fill in name and specialization.", "Error",

                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;

            }

            var doctor = new Doctor

            {

                Id = _view.Id,

                Name = _view.Name,

                Specialization = _view.Specialization

            };

            var result = await _apiClient.Save(doctor);

            if (result.HasError)

            {

                _view.ShowMessage($"Error saving doctor: {result.Error}", "Error",

                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            else

            {

                _view.ShowMessage("Doctor saved successfully.", "Success",

                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadDoctors();

            }

        }

        public void NewDoctor()

        {

            _view.ClearFields();

        }

        public void DoctorSelected()

        {

            if (_view.SelectedDoctor != null)

            {

                _view.Id = _view.SelectedDoctor.Id;

                _view.Name = _view.SelectedDoctor.Name;

                _view.Specialization = _view.SelectedDoctor.Specialization;

            }

            else

            {

                _view.ClearFields();

            }

        }

    }

}
