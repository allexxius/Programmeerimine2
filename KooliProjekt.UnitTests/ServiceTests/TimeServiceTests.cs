using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class TimeServiceTests : ServiceTestBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TimeService _timeService;

        public TimeServiceTests()
        {
            _context = DbContext;
            _timeService = new TimeService(_context);

            // Add test data
            _context.Times.AddRange(new List<Time>
            {
                new Time { Id = 1, Date = DateTime.Now, VisitTime = new TimeOnly(10, 0), Free = true, DoctorId = 1 },
                new Time { Id = 2, Date = DateTime.Now, VisitTime = new TimeOnly(11, 0), Free = false, DoctorId = 2 },
                new Time { Id = 3, Date = DateTime.Now, VisitTime = new TimeOnly(12, 0), Free = true, DoctorId = 3 }
            });
            _context.SaveChanges();
        }

        [Fact]
        public async Task List_WithoutFilters_ReturnsAllTimes()
        {
            var page = 1;
            var pageSize = 10;

            var result = await _timeService.List(page, pageSize);

            Assert.Equal(3, result.Results.Count);
        }

        [Fact]
        public async Task Get_ExistingTimeId_ReturnsTime()
        {
            var timeId = 3;

            var result = await _timeService.Get(timeId);

            Assert.NotNull(result);
            Assert.Equal(12, result.VisitTime.Hour);
        }

        [Fact]
        public async Task Get_NonExistingTimeId_ReturnsNull()
        {
            var timeId = 999;

            var result = await _timeService.Get(timeId);

            Assert.Null(result);
        }

        [Fact]
        public async Task Save_NewTime_AddsTimeToDatabase()
        {
            var newTime = new Time { Date = DateTime.Now, VisitTime = new TimeOnly(13, 0), Free = true, DoctorId = 4 };

            await _timeService.Save(newTime);
            await _context.SaveChangesAsync();

            var result = await _context.Times.FindAsync(newTime.Id);

            Assert.NotNull(result);
            Assert.Equal(13, result.VisitTime.Hour);
        }

        [Fact]
        public async Task Save_ExistingTime_UpdatesTimeInDatabase()
        {
            var existingTime = await _context.Times.FindAsync(1);
            existingTime.Free = false;

            await _timeService.Save(existingTime);
            await _context.SaveChangesAsync();

            var result = await _context.Times.FindAsync(1);

            Assert.NotNull(result);
            Assert.False(result.Free);
        }

        [Fact]
        public async Task Delete_ExistingTimeId_RemovesTimeFromDatabase()
        {
            var timeId = 2;

            await _timeService.Delete(timeId);
            await _context.SaveChangesAsync();

            var result = await _context.Times.FindAsync(timeId);

            Assert.Null(result);
        }

        [Fact]
        public async Task Delete_NonExistingTimeId_DoesNothing()
        {
            var timeId = 999;

            await _timeService.Delete(timeId);
            await _context.SaveChangesAsync();

            var result = await _context.Times.FindAsync(timeId);

            Assert.Null(result);
        }
    }
}
