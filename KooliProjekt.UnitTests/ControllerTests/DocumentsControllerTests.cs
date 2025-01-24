using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class DocumentsControllerTests
    {
        private readonly Mock<IDocumentService> _documentServiceMock;
        private readonly DocumentsController _controller;

        public DocumentsControllerTests()
        {
            _documentServiceMock = new Mock<IDocumentService>();
            _controller = new DocumentsController(_documentServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<Document>
            {
                new Document { ID = 1, Type = "Test 1" },
                new Document { ID = 2, Type = "Test 2" }
            };
            var pagedResult = new PagedResult<Document> { Results = data };
            _documentServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);
        }
    }
}