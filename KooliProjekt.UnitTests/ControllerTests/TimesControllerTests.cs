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
    public class TimesControllerTests
    {
        private readonly Mock<ITimeService> _timeServiceMock;
        private readonly TimesController _controller;

        public TimesControllerTests()
        {
            _timeServiceMock = new Mock<ITimeService>();
            _controller = new TimesController(_timeServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<Time>
            {
                new Time { Id = 1, Free = true },
                new Time { Id = 2, Free = true }
            };
            var pagedResult = new PagedResult<Time> { Results = data };
            _timeServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);
        }
    }
}