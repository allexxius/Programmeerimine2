using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using KooliProjekt.Controllers;

using KooliProjekt.Data;

using KooliProjekt.Services;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests

{

    public class VisitsControllerTests

    {

        private readonly Mock<IVisitService> _visitServiceMock;

        private readonly VisitsController _controller;

        public VisitsControllerTests()

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

        [Fact]

        public async Task Edit_GET_should_return_not_found_for_invalid_id()

        {

            // Arrange

            int id = 999;

            _visitServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Visit)null);

            // Act

            var result = await _controller.Edit(id) as NotFoundResult;

            // Assert

            Assert.NotNull(result);

        }

        [Fact]

        public async Task Edit_GET_should_return_view_with_model_for_valid_id()

        {

            // Arrange

            int id = 1;

            var visit = new Visit { Id = id, Duration = 30 };

            _visitServiceMock.Setup(x => x.Get(id)).ReturnsAsync(visit);

            // Act

            var result = await _controller.Edit(id) as ViewResult;

            // Assert

            Assert.NotNull(result);

            Assert.Equal(visit, result.Model);

        }

        [Fact]

        public async Task Details_should_return_not_found_for_invalid_id()

        {

            // Arrange

            int id = 999;

            _visitServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Visit)null);

            // Act

            var result = await _controller.Details(id) as NotFoundResult;

            // Assert

            Assert.NotNull(result);

        }

        [Fact]

        public async Task Details_should_return_view_with_model_for_valid_id()

        {

            // Arrange

            int id = 1;

            var visit = new Visit { Id = id, Duration = 30 };

            _visitServiceMock.Setup(x => x.Get(id)).ReturnsAsync(visit);

            // Act

            var result = await _controller.Details(id) as ViewResult;

            // Assert

            Assert.NotNull(result);

            Assert.Equal(visit, result.Model);

        }

        [Fact]

        public void Create_GET_should_return_view()

        {

            // Act

            var result = _controller.Create() as ViewResult;

            // Assert

            Assert.NotNull(result);

        }

        [Fact]

        public async Task Delete_GET_should_return_not_found_for_invalid_id()

        {

            // Arrange

            int id = 999;

            _visitServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Visit)null);

            // Act

            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert

            Assert.NotNull(result);

        }

        [Fact]

        public async Task Delete_GET_should_return_view_with_model_for_valid_id()

        {

            // Arrange

            int id = 1;

            var visit = new Visit { Id = id, Duration = 30 };

            _visitServiceMock.Setup(x => x.Get(id)).ReturnsAsync(visit);

            // Act

            var result = await _controller.Delete(id) as ViewResult;

            // Assert

            Assert.NotNull(result);

            Assert.Equal(visit, result.Model);

        }

    }

}
