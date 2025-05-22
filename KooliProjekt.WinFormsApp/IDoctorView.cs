using KooliProjekt.WinFormsApp.Api;

using System.Collections.Generic;

namespace KooliProjekt.WinFormsApp

{

    public interface IDoctorView

    {

        IList<Doctor> Doctors { get; set; }

        Doctor SelectedDoctor { get; }

        string Name { get; set; }

        string Specialization { get; set; }

        int Id { get; set; }

        void ShowMessage(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);

        bool ConfirmDelete(string message, string caption);

        void ClearFields();

    }

}
