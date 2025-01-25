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

    public class InvoiceLinesControllerTests

    {

        private readonly Mock<IInvoiceLineService> _invoiceLineServiceMock;

        private readonly InvoiceLinesController _controller;

        public InvoiceLinesControllerTests()

        {

            _invoiceLineServiceMock = new Mock<IInvoiceLineService>();

            _controller = new InvoiceLinesController(_invoiceLineServiceMock.Object);

        }

        [Fact]

        public async Task Index_should_return_correct_view_with_data()

        {

            // Arrange

            int page = 1;

            var data = new List<InvoiceLine>

            {

                new InvoiceLine { Id = 1, Service = "Test 1" },

                new InvoiceLine { Id = 2, Service = "Test 2" }

            };

            var pagedResult = new PagedResult<InvoiceLine> { Results = data };

            _invoiceLineServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

            // Act

            var result = await _controller.Index(page) as ViewResult;

            // Assert

            Assert.NotNull(result);

            Assert.Equal(pagedResult, result.Model);

        }

    }

}
