using KooliProjekt.Data;

using KooliProjekt.Services;

using Microsoft.EntityFrameworkCore;

using System;

using System.Collections.Generic;

using System.Threading.Tasks;

using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests

{

    public class VisitServiceTests : ServiceTestBase

    {

        private readonly ApplicationDbContext _context;

        private readonly VisitService _visitService;

        public VisitServiceTests()

        {

            _context = DbContext;

            _visitService = new VisitService(_context);

            // Add test data

            _context.Visits.AddRange(new List<Visit>

            {

                new Visit { Id = 1, Name = "Visit A", UserId = "user1", DoctorId = 1, Date = DateTime.Now, Duration = 30 },

                new Visit { Id = 2, Name = "Visit B", UserId = "user2", DoctorId = 2, Date = DateTime.Now, Duration = 45 },

                new Visit { Id = 3, Name = "Visit C", UserId = "user3", DoctorId = 3, Date = DateTime.Now, Duration = 60 }

            });

            _context.SaveChanges();

        }

        [Fact]

        public async Task List_WithoutFilters_ReturnsAllVisits()

        {

            var page = 1;

            var pageSize = 10;

            var result = await _visitService.List(page, pageSize);

            Assert.Equal(3, result.Results.Count);

        }

        [Fact]

        public async Task Get_ExistingVisitId_ReturnsVisit()

        {

            var visitId = 2;

            var result = await _visitService.Get(visitId);

            Assert.NotNull(result);

            Assert.Equal("Visit B", result.Name);

        }

        [Fact]

        public async Task Get_NonExistingVisitId_ReturnsNull()

        {

            var visitId = 999;

            var result = await _visitService.Get(visitId);

            Assert.Null(result);

        }

        [Fact]

        public async Task Save_NewVisit_AddsVisitToDatabase()

        {

            var newVisit = new Visit { Name = "Visit D", UserId = "user4", DoctorId = 4, Date = DateTime.Now, Duration = 30 };

            await _visitService.Save(newVisit);

            await _context.SaveChangesAsync();

            var result = await _context.Visits.FindAsync(newVisit.Id);

            Assert.NotNull(result);

            Assert.Equal("Visit D", result.Name);

        }

        [Fact]

        public async Task Save_ExistingVisit_UpdatesVisitInDatabase()

        {

            var existingVisit = await _context.Visits.FindAsync(1);

            existingVisit.Duration = 90;

            await _visitService.Save(existingVisit);

            await _context.SaveChangesAsync();

            var result = await _context.Visits.FindAsync(1);

            Assert.NotNull(result);

            Assert.Equal(90, result.Duration);

        }

        [Fact]

        public async Task Delete_ExistingVisitId_RemovesVisitFromDatabase()

        {

            var visitId = 3;

            await _visitService.Delete(visitId);

            await _context.SaveChangesAsync();

            var result = await _context.Visits.FindAsync(visitId);

            Assert.Null(result);

        }

        [Fact]

        public async Task Delete_NonExistingVisitId_DoesNothing()

        {

            var visitId = 999;

            await _visitService.Delete(visitId);

            await _context.SaveChangesAsync();

            var result = await _context.Visits.FindAsync(visitId);

            Assert.Null(result);

        }

    }

}

