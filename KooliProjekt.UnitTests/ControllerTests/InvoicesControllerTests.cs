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

    public class InvoiceControllerTests

    {

        private readonly Mock<IInvoiceService> _invoiceServiceMock;

        private readonly InvoicesController _controller;

        public InvoiceControllerTests()

        {

            _invoiceServiceMock = new Mock<IInvoiceService>();

            _controller = new InvoicesController(_invoiceServiceMock.Object);

        }

        [Fact]

        public async Task Index_should_return_correct_view_with_data()

        {

            // Arrange

            int page = 1;

            var data = new List<Invoice>

            {

                new Invoice { Id = 1, Sum = 1 },

                new Invoice { Id = 2, Sum = 1 }

            };

            var pagedResult = new PagedResult<Invoice> { Results = data };

            _invoiceServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

            // Act

            var result = await _controller.Index(page) as ViewResult;

            // Assert

            Assert.NotNull(result);

            Assert.Equal(pagedResult, result.Model);

        }

    }

}
