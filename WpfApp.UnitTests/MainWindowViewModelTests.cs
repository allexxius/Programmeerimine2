using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using WpfApp.Api;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Reflection;

namespace WpfApp.UnitTests
{
    public class MainWindowViewModelTests
    {
        private readonly Mock<IApiClient> _mockApiClient;
        private readonly MainWindowViewModel _viewModel;

        public MainWindowViewModelTests()
        {
            _mockApiClient = new Mock<IApiClient>();
            _viewModel = new MainWindowViewModel(_mockApiClient.Object);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            Assert.NotNull(_viewModel.Lists);
            Assert.NotNull(_viewModel.NewCommand);
            Assert.NotNull(_viewModel.SaveCommand);
            Assert.NotNull(_viewModel.DeleteCommand);
            Assert.IsType<ObservableCollection<Doctor>>(_viewModel.Lists);
        }

        [Fact]
        public void Constructor_WithApiClient_InitializesCorrectly()
        {
            var mockApiClient = new Mock<IApiClient>();
            var vm = new MainWindowViewModel(mockApiClient.Object);
            Assert.NotNull(vm.Lists);
            Assert.NotNull(vm.NewCommand);
        }

        [Fact]
        public void SelectedItem_PropertyChanged_RaisesNotification()
        {
            var doctor = new Doctor { Id = 1, Name = "Dr. Smith" };
            var propertyChangedRaised = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MainWindowViewModel.SelectedItem))
                {
                    propertyChangedRaised = true;
                }
            };

            _viewModel.SelectedItem = doctor;
            Assert.True(propertyChangedRaised);
            Assert.Equal(doctor, _viewModel.SelectedItem);
        }

        [Fact]
        public async Task Load_SuccessfulApiCall_PopulatesLists()
        {
            var doctors = new List<Doctor>
            {
                new Doctor { Id = 1, Name = "Dr. Smith" },
                new Doctor { Id = 2, Name = "Dr. Johnson" }
            };

            _mockApiClient.Setup(x => x.List())
                         .ReturnsAsync(new Result<List<Doctor>> { Value = doctors });

            await _viewModel.Load();

            Assert.Equal(2, _viewModel.Lists.Count);
            Assert.Equal(doctors[0].Name, _viewModel.Lists[0].Name);
            Assert.Equal(doctors[1].Name, _viewModel.Lists[1].Name);
            _mockApiClient.Verify(x => x.List(), Times.Once);
        }

        [Fact]
        public async Task Load_FailedApiCall_InvokesOnError()
        {
            var errorMessage = "API Error";
            _mockApiClient.Setup(x => x.List())
                         .ReturnsAsync(new Result<List<Doctor>> { Error = errorMessage });

            string receivedError = null;
            _viewModel.OnError = (error) => receivedError = error;

            await _viewModel.Load();

            Assert.Equal(errorMessage, receivedError);
            Assert.Empty(_viewModel.Lists);
        }

        [Fact]
        public void NewCommand_Execute_CreatesNewSelectedItem()
        {
            _viewModel.SelectedItem = null;
            _viewModel.NewCommand.Execute(null);

            Assert.NotNull(_viewModel.SelectedItem);
            Assert.IsType<Doctor>(_viewModel.SelectedItem);
            Assert.Equal(0, _viewModel.SelectedItem.Id);
            Assert.Null(_viewModel.SelectedItem.Name);
        }

        [Fact]
        public void SaveCommand_CanExecute_ReturnsCorrectValues()
        {
            Assert.False(_viewModel.SaveCommand.CanExecute(null));
            _viewModel.SelectedItem = new Doctor();
            Assert.True(_viewModel.SaveCommand.CanExecute(null));
            _viewModel.SelectedItem = null;
            Assert.False(_viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public async Task SaveCommand_Execute_SavesAndReloads()
        {
            var doctor = new Doctor { Id = 1, Name = "Dr. Smith" };
            _viewModel.SelectedItem = doctor;

            var doctors = new List<Doctor> { doctor };
            _mockApiClient.Setup(x => x.List())
                         .ReturnsAsync(new Result<List<Doctor>> { Value = doctors });

            // Fix: Return a Task<Result> instead of Task.CompletedTask
            _mockApiClient.Setup(x => x.Save(doctor))
                         .ReturnsAsync(new Result());

            await _viewModel.SaveAsync();

            _mockApiClient.Verify(x => x.Save(doctor), Times.Once);
            _mockApiClient.Verify(x => x.List(), Times.Once);
            Assert.Single(_viewModel.Lists);
            Assert.Equal(doctor.Name, _viewModel.Lists[0].Name);
        }

        [Fact]
        public void DeleteCommand_CanExecute_ReturnsCorrectValues()
        {
            Assert.False(_viewModel.DeleteCommand.CanExecute(null));
            _viewModel.SelectedItem = new Doctor();
            Assert.True(_viewModel.DeleteCommand.CanExecute(null));
            _viewModel.SelectedItem = null;
            Assert.False(_viewModel.DeleteCommand.CanExecute(null));
        }

        [Fact]
        public async Task DeleteCommand_Execute_WithUserConfirmation_DeletesItem()
        {
            var doctor = new Doctor { Id = 1, Name = "Dr. Smith" };
            _viewModel.SelectedItem = doctor;
            _viewModel.Lists.Add(doctor);

            _viewModel.ConfirmDelete = (d) => true;

            // Fix: Return a Task<Result> instead of Task.CompletedTask
            _mockApiClient.Setup(x => x.Delete(doctor.Id))
                         .ReturnsAsync(new Result());

            await _viewModel.DeleteAsync();

            _mockApiClient.Verify(x => x.Delete(doctor.Id), Times.Once);
            Assert.Empty(_viewModel.Lists);
            Assert.Null(_viewModel.SelectedItem);
        }

        [Fact]
        public async Task DeleteCommand_Execute_WithoutUserConfirmation_DoesNotDelete()
        {
            var doctor = new Doctor { Id = 1, Name = "Dr. Smith" };
            _viewModel.SelectedItem = doctor;
            _viewModel.Lists.Add(doctor);

            _viewModel.ConfirmDelete = (d) => false;

            await _viewModel.DeleteAsync();

            _mockApiClient.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
            Assert.Single(_viewModel.Lists);
            Assert.NotNull(_viewModel.SelectedItem);
        }

        [Fact]
        public async Task DeleteCommand_Execute_WithoutConfirmationHandler_DeletesItem()
        {
            var doctor = new Doctor { Id = 1, Name = "Dr. Smith" };
            _viewModel.SelectedItem = doctor;
            _viewModel.Lists.Add(doctor);

            _viewModel.ConfirmDelete = null;

            // Fix: Return a Task<Result> instead of Task.CompletedTask
            _mockApiClient.Setup(x => x.Delete(doctor.Id))
                         .ReturnsAsync(new Result());

            await _viewModel.DeleteAsync();

            _mockApiClient.Verify(x => x.Delete(doctor.Id), Times.Once);
            Assert.Empty(_viewModel.Lists);
            Assert.Null(_viewModel.SelectedItem);
        }

        [Fact]
        public void DefaultConstructor_InitializesApiClient()
        {
            var vm = new MainWindowViewModel();
            Assert.NotNull(vm.Lists);
            Assert.NotNull(vm.NewCommand);

            var field = typeof(MainWindowViewModel).GetField("_apiClient", BindingFlags.NonPublic | BindingFlags.Instance);
            var apiClient = field.GetValue(vm);
            Assert.IsType<ApiClient>(apiClient);
        }
    }
}