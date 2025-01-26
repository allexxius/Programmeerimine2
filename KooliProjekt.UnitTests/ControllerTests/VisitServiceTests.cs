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

    public class VisitControllerTests

    {

        private readonly Mock<IVisitService> _visitServiceMock;

        private readonly VisitsController _controller;

        public VisitControllerTests()

        {

            _visitServiceMock = new Mock<IVisitService>();

            _controller = new VisitsController(_visitServiceMock.Object);

        }

        [Fact]

        public async Task Index_should_return_correct_view_with_data()

        {

            // Arrange

            int page = 1;

            var data = new List<Visit>

            {

                new Visit { Id = 1, Duration = 1 },

                new Visit { Id = 2, Duration = 2 }

            };

            var pagedResult = new PagedResult<Visit> { Results = data };

            _visitServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

            // Act

            var result = await _controller.Index(page) as ViewResult;

            // Assert

            Assert.NotNull(result);

            Assert.Equal(pagedResult, result.Model);

        }

    }

}
