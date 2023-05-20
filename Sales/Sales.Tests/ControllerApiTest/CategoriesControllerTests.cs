using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Controllers;
using Sales.API.Data;
using Sales.Shared.DTOs;
using Sales.Shared.Entities;

namespace Sales.Tests.ControllerApiTest
{
    public class CategoriesControllerTests
    {
        private readonly CategoriesController _controller;
        private readonly DataContext _context;


        public CategoriesControllerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "testDatabase")
                .Options;

            _context = new DataContext(options);
            _controller = new CategoriesController(_context);
        }

        [Fact]
        public async Task Get_ReturnsOkResult()
        {
            // Arrange
            var pagination = new PaginationDTO { Page = 1, RecordsNumber = 10 };

            // Act
            var result = await _controller.GetAsync(pagination);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsOkResultWithFilter()
        {
            // Arrange
            var pagination = new PaginationDTO { Page = 1, RecordsNumber = 10, Filter = "Any" };

            // Act
            var result = await _controller.GetAsync(pagination);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            var id = 999;

            // Act
            var result = await _controller.GetAsync(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenCategoryExists()
        {
            // Arrange
            var category = new Category { Id = 666, Name = "Test" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAsync(category.Id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenCategoryNameAlreadyExists()
        {
            // Arrange
            var category = new Category { Name = "Test" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.PostAsync(category);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_WhenCategoryNameAlreadyExists()
        {
            // Arrange
            var category = new Category { Id = 100, Name = "Category1" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var categoryToUpdate = new Category { Id = 200, Name = "Category1" };

            // Act
            var result = await _controller.PutAsync(categoryToUpdate);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenCategoryIsDeleted()
        {
            // Arrange
            var category = new Category { Id = 123, Name = "Category1" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteAsync(category.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
