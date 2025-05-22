using System;

using System.Collections.ObjectModel;

using System.Net.Http;

using System.Threading.Tasks;

using System.Windows.Input;

using WpfApp.Api;

using WpfApp.Commands;

namespace WpfApp.ViewModels

{

    public class MainWindowViewModel : NotifyPropertyChangedBase

    {

        private readonly IApiClient _apiClient;

        private Doctor _selectedItem;

        public ObservableCollection<Doctor> Lists { get; } = new ObservableCollection<Doctor>();

        public ICommand NewCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand DeleteCommand { get; }

        public Predicate<Doctor> ConfirmDelete { get; set; }

        public Action<string> OnError { get; set; }

        public Doctor SelectedItem

        {

            get => _selectedItem;

            set

            {

                _selectedItem = value;

                NotifyPropertyChanged();

                CommandManager.InvalidateRequerySuggested();

            }

        }

        public MainWindowViewModel(IApiClient apiClient)

        {

            _apiClient = apiClient;

            NewCommand = new RelayCommand(_ => New());

            SaveCommand = new RelayCommand(async _ => await SaveAsync(), _ => CanSave());

            DeleteCommand = new RelayCommand(async _ => await DeleteAsync(), _ => CanDelete());

        }

        public MainWindowViewModel() : this(new ApiClient(new HttpClient())) { }

        public async Task Load()

        {

            var result = await _apiClient.List();

            if (result.HasError)

            {

                OnError?.Invoke(result.Error);

                return;

            }

            Lists.Clear();

            foreach (var doctor in result.Value)

            {

                Lists.Add(doctor);

            }

        }

        private void New()

        {

            SelectedItem = new Doctor { Id = 0 };

        }

        private bool CanSave() => SelectedItem != null;

        private bool CanDelete() => SelectedItem != null;

        public async Task SaveAsync()

        {

            if (SelectedItem == null) return;

            var result = await _apiClient.Save(SelectedItem);

            if (result.HasError)

            {

                OnError?.Invoke(result.Error);

                return;

            }

            await Load();

        }

        public async Task DeleteAsync()

        {

            if (SelectedItem == null) return;

            if (ConfirmDelete != null && !ConfirmDelete(SelectedItem))

            {

                return;

            }

            var result = await _apiClient.Delete(SelectedItem.Id);

            if (result.HasError)

            {

                OnError?.Invoke(result.Error);

                return;

            }

            Lists.Remove(SelectedItem);

            SelectedItem = null;

        }

    }

}
