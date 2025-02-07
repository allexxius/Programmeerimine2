using KooliProjekt.Data;

using KooliProjekt.Services;

using Microsoft.EntityFrameworkCore;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests

{

    public class DocumentServiceTests : IDisposable

    {

        private readonly ApplicationDbContext _context;

        private readonly DocumentService _documentService;

        public DocumentServiceTests()

        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()

                .UseInMemoryDatabase(databaseName: "TestDatabase")

                .Options;

            _context = new ApplicationDbContext(options);

            _documentService = new DocumentService(_context);

        }

        [Fact]

        public async Task List_ReturnsPagedResult()

        {

            // Arrange

            _context.Documents.AddRange(new List<Document>

            {

                new Document { ID = 1, Type = "PDF", File = "file1.pdf", Visit = 1 },

                new Document { ID = 2, Type = "DOC", File = "file2.doc", Visit = 2 }

            });

            await _context.SaveChangesAsync();

            // Act

            var result = await _documentService.List(1, 5);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(2, result.Results.Count());

        }

        [Fact]

        public async Task Get_ReturnsDocument()

        {

            // Arrange

            var document = new Document { ID = 1, Type = "PDF", File = "file1.pdf", Visit = 1 };

            _context.Documents.Add(document);

            await _context.SaveChangesAsync();

            // Act

            var result = await _documentService.Get(1);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(1, result.ID);

        }

        [Fact]

        public async Task Save_AddsNewDocument()

        {

            // Arrange

            var document = new Document { ID = 0, Type = "PDF", File = "file1.pdf", Visit = 1 };

            // Act

            await _documentService.Save(document);

            // Assert

            Assert.Single(_context.Documents);

        }

        [Fact]

        public async Task Save_UpdatesExistingDocument()

        {

            // Arrange

            var document = new Document { ID = 1, Type = "PDF", File = "file1.pdf", Visit = 1 };

            _context.Documents.Add(document);

            await _context.SaveChangesAsync();

            document.File = "updated_file.pdf";

            // Act

            await _documentService.Save(document);

            // Assert

            Assert.Equal("updated_file.pdf", _context.Documents.First().File);

        }

        [Fact]

        public async Task Delete_RemovesDocument()

        {

            // Arrange

            var document = new Document { ID = 1, Type = "PDF", File = "file1.pdf", Visit = 1 };

            _context.Documents.Add(document);

            await _context.SaveChangesAsync();

            // Act

            await _documentService.Delete(1);

            // Assert

            Assert.Empty(_context.Documents);

        }

        public void Dispose()

        {

            _context.Database.EnsureDeleted();

            _context.Dispose();

        }

    }

}

