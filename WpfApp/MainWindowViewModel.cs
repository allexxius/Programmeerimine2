using System;

using System.Collections.ObjectModel;

using System.Windows.Input;

using WpfApp.Api;

namespace WpfApp

{

    public class MainWindowViewModel : NotifyPropertyChangedBase

    {

        public ObservableCollection<Doctor> Lists { get; private set; }

        public ICommand NewCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }

        public ICommand DeleteCommand { get; private set; }

        public Predicate<Doctor> ConfirmDelete { get; set; }

        public Action<string> OnError { get; set; }

        private readonly IApiClient _apiClient;

        public MainWindowViewModel() : this(new ApiClient())

        {

        }

        public MainWindowViewModel(IApiClient apiClient)

        {

            _apiClient = apiClient;

            Lists = new ObservableCollection<Doctor>();

            NewCommand = new RelayCommand<Doctor>(

                // Execute

                list =>

                {

                    SelectedItem = new Doctor();

                }

            );

            SaveCommand = new RelayCommand<Doctor>(

                // Execute

                async list =>

                {

                    await SaveAsync();

                },

                // CanExecute

                list =>

                {

                    return SelectedItem != null;

                }

            );

            DeleteCommand = new RelayCommand<Doctor>(

                // Execute

                async list =>

                {

                    await DeleteAsync();

                },

                // CanExecute

                list =>

                {

                    return SelectedItem != null;

                }

            );

        }

        public async Task Load()

        {

            Lists.Clear();

            var lists = await _apiClient.List();

            if (lists.HasError)

            {

                if (OnError != null)

                {

                    OnError(lists.Error);

                }

                return;

            }

            foreach (var list in lists.Value)

            {

                Lists.Add(list);

            }

        }

        public async Task DeleteAsync()

        {

            if (SelectedItem == null) return;

            if (ConfirmDelete != null)

            {

                var result = ConfirmDelete(SelectedItem);

                if (!result)

                {

                    return;

                }

            }

            await _apiClient.Delete(SelectedItem.Id);

            Lists.Remove(SelectedItem);

            SelectedItem = null;

        }

        public async Task SaveAsync()

        {

            if (SelectedItem == null) return;

            await _apiClient.Save(SelectedItem);

            await Load();

        }

        private Doctor _selectedItem;

        public Doctor SelectedItem

        {

            get

            {

                return _selectedItem;

            }

            set

            {

                _selectedItem = value;

                NotifyPropertyChanged();

            }

        }

    }

}
