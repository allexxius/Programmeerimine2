using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using KooliProjekt.UnitTests.ServiceTests;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class DoctorServiceTests : ServiceTestBase
    {
        private ApplicationDbContext _context;
        private DoctorService _doctorService;

        public DoctorServiceTests()
        {
            _context = DbContext;
            _doctorService = new DoctorService(_context);

            // Add test data
            _context.Doctors.AddRange(new List<Doctor>
            {
                new Doctor { Id = 1, Name = "Dr. Smith", Specialization = "Cardiology", UserId = 1 },
                new Doctor { Id = 2, Name = "Dr. Johnson", Specialization = "Neurology", UserId = 2 },
                new Doctor { Id = 3, Name = "Dr. Williams", Specialization = "Dermatology", UserId = 3 }
            });
            _context.SaveChanges();
        }

        [Fact]
        public async Task List_WithoutSearchParameters_ReturnsAllDoctors()
        {
            var page = 1;
            var pageSize = 10;

            var result = await _doctorService.List(page, pageSize);

            Assert.Equal(3, result.Results.Count);
        }

        [Fact]
        public async Task List_WithSearchParameters_ReturnsFilteredDoctors()
        {
            var page = 1;
            var pageSize = 10;
            var search = new DoctorSearch { Keyword = "Smith" };

            var result = await _doctorService.List(page, pageSize, search);

            Assert.Equal(1, result.Results.Count);
            Assert.Equal("Dr. Smith", result.Results.First().Name);
        }

        [Fact]
        public async Task Get_ExistingDoctorId_ReturnsDoctor()
        {
            var doctorId = 1;

            var result = await _doctorService.Get(doctorId);

            Assert.NotNull(result);
            Assert.Equal("Dr. Smith", result.Name);
        }

        [Fact]
        public async Task Get_NonExistingDoctorId_ReturnsNull()
        {
            var doctorId = 999;

            var result = await _doctorService.Get(doctorId);

            Assert.Null(result);
        }

        [Fact]
        public async Task Save_NewDoctor_AddsDoctorToDatabase()
        {
            var newDoctor = new Doctor { Name = "Dr. Brown", Specialization = "Pediatrics", UserId = 4 };

            await _doctorService.Save(newDoctor);
            await _context.SaveChangesAsync();  // Ensure changes are persisted

            var result = await _context.Doctors.FindAsync(newDoctor.Id);

            Assert.NotNull(result);
            Assert.Equal("Dr. Brown", result.Name);
        }

        [Fact]
        public async Task Save_ExistingDoctor_UpdatesDoctorInDatabase()
        {
            var existingDoctor = await _context.Doctors.FindAsync(1);
            existingDoctor.Name = "Dr. Smith Updated";

            await _doctorService.Save(existingDoctor);
            await _context.SaveChangesAsync();  // Ensure changes are persisted

            var result = await _context.Doctors.FindAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Dr. Smith Updated", result.Name);
        }

        [Fact]
        public async Task Delete_ExistingDoctorId_RemovesDoctorFromDatabase()
        {
            var doctorId = 1;

            await _doctorService.Delete(doctorId);
            await _context.SaveChangesAsync();  // Ensure changes are persisted

            var result = await _context.Doctors.FindAsync(doctorId);

            Assert.Null(result);
        }

        [Fact]
        public async Task Delete_NonExistingDoctorId_DoesNothing()
        {
            var doctorId = 999;

            await _doctorService.Delete(doctorId);
            await _context.SaveChangesAsync();  // Ensure changes are persisted

            var result = await _context.Doctors.FindAsync(doctorId);

            Assert.Null(result);
        }
    }
}
