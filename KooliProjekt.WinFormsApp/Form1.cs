using KooliProjekt.WinFormsApp.Api;

using System.Collections.Generic;

using System.Windows.Forms;

namespace KooliProjekt.WinFormsApp

{

    public partial class Form1 : Form, IDoctorView

    {

        private readonly IDoctorPresenter _presenter;

        private readonly BindingSource _bindingSource = new BindingSource();

        public Form1()

        {

            InitializeComponent();

            _presenter = new DoctorPresenter(this, new ApiClient());

            InitializeDataGridView();

        }

        protected override async void OnLoad(EventArgs e)

        {

            base.OnLoad(e);

            await _presenter.Initialize();

        }

        #region IDoctorView Implementation

        public IList<Doctor> Doctors

        {

            get => (IList<Doctor>)_bindingSource.DataSource;

            set => _bindingSource.DataSource = value;

        }

        public Doctor SelectedDoctor => DoctorsGrid.SelectedRows.Count > 0

            ? (Doctor)DoctorsGrid.SelectedRows[0].DataBoundItem

            : null;

        public string Name

        {

            get => NameField.Text;

            set => NameField.Text = value;

        }

        public string Specialization

        {

            get => SpecializationField.Text;

            set => SpecializationField.Text = value;

        }

        public int Id

        {

            get => int.TryParse(IdField.Text, out int id) ? id : 0;

            set => IdField.Text = value.ToString();

        }

        public void ShowMessage(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)

        {

            MessageBox.Show(this, message, caption, buttons, icon);

        }

        public bool ConfirmDelete(string message, string caption)

        {

            return MessageBox.Show(this, message, caption,

                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

        }

        public void ClearFields()

        {

            IdField.Text = "0";

            NameField.Text = string.Empty;

            SpecializationField.Text = string.Empty;

            DoctorsGrid.ClearSelection();

        }

        #endregion

        #region Event Handlers

        private void DoctorsGrid_SelectionChanged(object sender, System.EventArgs e)

        {

            _presenter.DoctorSelected();

        }

        private async void SaveButton_Click(object sender, System.EventArgs e)

        {

            await _presenter.SaveDoctor();

        }

        private async void DeleteButton_Click(object sender, System.EventArgs e)

        {

            await _presenter.DeleteDoctor();

        }

        private void NewButton_Click(object sender, System.EventArgs e)

        {

            _presenter.NewDoctor();

        }

        #endregion

        private void InitializeDataGridView()

        {

            DoctorsGrid.AutoGenerateColumns = false;

            DoctorsGrid.Columns.Clear();

            DoctorsGrid.Columns.Add(new DataGridViewTextBoxColumn()

            {

                DataPropertyName = "Id",

                HeaderText = "ID",

                Width = 50

            });

            DoctorsGrid.Columns.Add(new DataGridViewTextBoxColumn()

            {

                DataPropertyName = "Name",

                HeaderText = "Name",

                Width = 150,

                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            });

            DoctorsGrid.Columns.Add(new DataGridViewTextBoxColumn()

            {

                DataPropertyName = "Specialization",

                HeaderText = "Specialization",

                Width = 200

            });

            DoctorsGrid.DataSource = _bindingSource;

        }

    }

}
