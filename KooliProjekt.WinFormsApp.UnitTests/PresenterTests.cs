using KooliProjekt.WinFormsApp.Api;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xunit;

namespace KooliProjekt.WinFormsApp.Tests
{
    public class DoctorPresenterTests
    {
        private readonly Mock<IDoctorView> _mockView;
        private readonly Mock<IApiClient> _mockApiClient;
        private readonly DoctorPresenter _presenter;

        public DoctorPresenterTests()
        {
            _mockView = new Mock<IDoctorView>();
            _mockApiClient = new Mock<IApiClient>();
            _presenter = new DoctorPresenter(_mockView.Object, _mockApiClient.Object);
        }

        private void SetupSuccessfulListResponse()
        {
            var doctors = new List<Doctor>
            {
                new Doctor { Id = 1, Name = "Dr. Smith", Specialization = "Cardiology" }
            };
            var result = new Result<List<Doctor>> { Value = doctors };
            _mockApiClient.Setup(x => x.List()).ReturnsAsync(result);
        }

        [Fact]
        public async Task InitializeShouldLoadDoctors()
        {
            // Arrange
            SetupSuccessfulListResponse();

            // Act
            await _presenter.Initialize();

            // Assert
            _mockApiClient.Verify(x => x.List(), Times.Once);
            _mockView.VerifySet(x => x.Doctors = It.IsAny<List<Doctor>>(), Times.Once);
        }

        [Fact]
        public async Task Initialize_WhenApiFails_ShouldShowError()
        {
            // Arrange
            var errorResult = new Result<List<Doctor>> { Error = "API Error" };
            _mockApiClient.Setup(x => x.List()).ReturnsAsync(errorResult);

            // Act
            await _presenter.Initialize();

            // Assert
            _mockView.Verify(x => x.ShowMessage(
                "Error loading doctors: API Error",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error),
                Times.Once);
        }

        [Fact]
        public async Task DeleteDoctor_WhenNoDoctorSelected_ShouldShowWarning()
        {
            // Arrange
            _mockView.SetupGet(x => x.Id).Returns(0);

            // Act
            await _presenter.DeleteDoctor();

            // Assert
            _mockView.Verify(x => x.ShowMessage(
                "Please select a doctor to delete.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning),
                Times.Once);
            _mockApiClient.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteDoctor_WhenUserConfirms_ShouldCallApiAndShowSuccess()
        {
            // Arrange
            const int doctorId = 1;
            _mockView.SetupGet(x => x.Id).Returns(doctorId);
            _mockView.Setup(x => x.ConfirmDelete(It.IsAny<string>(), It.IsAny<string>()))
                   .Returns(true);

            // Setup both Delete and List responses
            _mockApiClient.Setup(x => x.Delete(doctorId))
                         .ReturnsAsync(new Result());
            SetupSuccessfulListResponse();

            // Act
            await _presenter.DeleteDoctor();

            // Assert
            _mockApiClient.Verify(x => x.Delete(doctorId), Times.Once);
            _mockApiClient.Verify(x => x.List(), Times.Once);
            _mockView.Verify(x => x.ShowMessage(
                "Doctor deleted successfully.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information),
                Times.Once);
            _mockView.Verify(x => x.ClearFields(), Times.Once);
        }

        [Fact]
        public async Task SaveDoctor_WhenFieldsEmpty_ShouldShowWarning()
        {
            // Arrange
            _mockView.SetupGet(x => x.Name).Returns("");
            _mockView.SetupGet(x => x.Specialization).Returns("");

            // Act
            await _presenter.SaveDoctor();

            // Assert
            _mockView.Verify(x => x.ShowMessage(
                "Please fill in name and specialization.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning),
                Times.Once);
            _mockApiClient.Verify(x => x.Save(It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public async Task SaveDoctor_WhenValid_ShouldCallApiAndShowSuccess()
        {
            // Arrange
            const int doctorId = 1;
            const string name = "Dr. Smith";
            const string specialization = "Cardiology";

            _mockView.SetupGet(x => x.Id).Returns(doctorId);
            _mockView.SetupGet(x => x.Name).Returns(name);
            _mockView.SetupGet(x => x.Specialization).Returns(specialization);

            // Setup both Save and List responses
            _mockApiClient.Setup(x => x.Save(It.Is<Doctor>(d =>
                d.Id == doctorId &&
                d.Name == name &&
                d.Specialization == specialization)))
                .ReturnsAsync(new Result());
            SetupSuccessfulListResponse();

            // Act
            await _presenter.SaveDoctor();

            // Assert   
            _mockApiClient.Verify(x => x.Save(It.Is<Doctor>(d =>
                d.Id == doctorId &&
                d.Name == name &&
                d.Specialization == specialization)),
                Times.Once);
            _mockApiClient.Verify(x => x.List(), Times.Once);
            _mockView.Verify(x => x.ShowMessage(
                "Doctor saved successfully.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information),
                Times.Once);
        }

        [Fact]
        public void NewDoctor_ShouldClearFields()
        {
            // Act
            _presenter.NewDoctor();

            // Assert
            _mockView.Verify(x => x.ClearFields(), Times.Once);
        }

        [Fact]
        public void DoctorSelected_WhenDoctorSelected_ShouldUpdateView()
        {
            // Arrange
            var doctor = new Doctor { Id = 1, Name = "Dr. Smith", Specialization = "Cardiology" };
            _mockView.SetupGet(x => x.SelectedDoctor).Returns(doctor);

            // Act
            _presenter.DoctorSelected();

            // Assert
            _mockView.VerifySet(x => x.Id = doctor.Id, Times.Once);
            _mockView.VerifySet(x => x.Name = doctor.Name, Times.Once);
            _mockView.VerifySet(x => x.Specialization = doctor.Specialization, Times.Once);
        }

        [Fact]
        public void DoctorSelected_WhenNoDoctorSelected_ShouldClearFields()
        {
            // Arrange
            _mockView.SetupGet(x => x.SelectedDoctor).Returns((Doctor)null);

            // Act
            _presenter.DoctorSelected();

            // Assert
            _mockView.Verify(x => x.ClearFields(), Times.Once);
        }
    }
}