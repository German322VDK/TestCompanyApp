using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TestCompanyApp.API.Controllers;
using TestCompanyApp.Domain.Entity;
using TestCompanyApp.Domain.ViewModels;
using TestCompanyApp.Infrastructure.Services.Stores.Employees;

namespace TestCompanyApp.API.Tests.Controllers
{
    public class EmployeesControllerTests
    {
        private readonly EmployeesController _controller;
        private readonly Mock<IEmployeesStore> _employeesStore;
        private readonly Mock<ILogger<EmployeesController>> _logger;

        private const int MIN_COUNT = 1;
        private const int MAX_COUNT = 1000;

        public EmployeesControllerTests()
        {
            _logger = new Mock<ILogger<EmployeesController>>();
            _employeesStore = new Mock<IEmployeesStore>();
            _controller = new EmployeesController(_employeesStore.Object, _logger.Object);
        }

        #region Add

        [Fact]
        public async Task Add_ShouldReturnOk_WhenEmployeeIsSuccessfullyAdded()
        {
            // Arrange
            AddedEmployeeViewModel model = new AddedEmployeeViewModel
            {
                FirstName = "Игорь",
                Patronymic = "Игоревич",
                SurName = "Игорев",
                JobTitle = Position.Manager,
                LeaderId = 1
            };
            Employee addedEmployee = new Employee { Id = 1, FirstName = model.FirstName, Patronymic = model.Patronymic, SurName = model.SurName, JobTitle = model.JobTitle };

            _employeesStore.Setup(store => store.AddAsync(model)).ReturnsAsync(addedEmployee);

            // Act
            IActionResult result = await _controller.Add(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEmployee = Assert.IsType<EmployeeViewModel>(okResult.Value);
            Assert.Equal(addedEmployee.FirstName, returnedEmployee.FirstName);
            Assert.Equal(addedEmployee.Patronymic, returnedEmployee.Patronymic);
            Assert.Equal(addedEmployee.SurName, returnedEmployee.SurName);
        }

        [Fact]
        public async Task Add_ShouldReturnBadRequest_WhenLeaderIdIsNull()
        {
            // Arrange
            var model = new AddedEmployeeViewModel
            {
                FirstName = "Анна",
                Patronymic = "Сергеевна",
                SurName = "Петрова",
                JobTitle = Position.SupportSpecialist,
                LeaderId = null // Руководитель не указан
            };

            // Act
            IActionResult result = await _controller.Add(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Не указан руководитель", badRequestResult.Value);
        }

        [Fact]
        public async Task Add_ShouldReturnBadRequest_WhenAddingEmployeeFails()
        {
            // Arrange
            var model = new AddedEmployeeViewModel
            {
                FirstName = "Елена",
                Patronymic = "Викторовна",
                SurName = "Сидорова",
                JobTitle = Position.Director,
                LeaderId = 2
            };

            _employeesStore.Setup(store => store.AddAsync(model)).ThrowsAsync(new ArgumentException("Ошибка при добавлении"));

            // Act
            IActionResult result = await _controller.Add(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Ошибка при добавлении", badRequestResult.Value);
        }


        #endregion

        #region Fire

        [Fact]
        public async Task Fire_ShouldReturnOk_WhenEmployeeIsSuccessfullyFired()
        {
            // Arrange
            int employeeId = 1;
            var employee = new Employee { Id = employeeId, FirstName = "Иван", Patronymic = "Иванович", SurName = "Иванов", JobTitle = Position.Director, IsEmployed = true };

            _employeesStore.Setup(store => store.GetById(employeeId)).Returns(employee);
            _employeesStore.Setup(store => store.FireAsync(employeeId, null)).ReturnsAsync(true);

            // Act
            IActionResult result = await _controller.Fire(employeeId, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task Fire_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            int employeeId = 999;
            _employeesStore.Setup(store => store.GetById(employeeId)).Returns((Employee?)null);

            // Act
            IActionResult result = await _controller.Fire(employeeId, null);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Уволняемый сотрудник не найден", notFoundResult.Value);
        }

        [Fact]
        public async Task Fire_ShouldReturnOk_WhenEmployeeIsAlreadyFired()
        {
            // Arrange
            int employeeId = 2;
            var employee = new Employee { Id = employeeId, FirstName = "Анна", Patronymic = "Петровна", SurName = "Петрова", JobTitle = Position.SupportSpecialist, IsEmployed = false };

            _employeesStore.Setup(store => store.GetById(employeeId)).Returns(employee);
            _employeesStore.Setup(store => store.FireAsync(employeeId, null)).ReturnsAsync(false);
            // Act
            IActionResult result = await _controller.Fire(employeeId, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.False((bool)okResult.Value); // Метод должен вернуть false, так как сотрудник уже уволен
        }



        #endregion

        #region Delete

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenEmployeeExists()
        {
            // Arrange
            int employeeId = 1;
            var employee = new Employee { Id = employeeId, FirstName = "Иван", Patronymic = "Иванович", SurName = "Иванов", JobTitle = Position.Director };
            _employeesStore.Setup(store => store.GetById(employeeId)).Returns(employee);
            _employeesStore.Setup(store => store.DeleteAsync(employeeId)).ReturnsAsync(true);

            // Act
            IActionResult result = await _controller.Delete(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            int employeeId = 999;
            _employeesStore.Setup(store => store.GetById(employeeId)).Returns((Employee?)null);

            // Act
            IActionResult result = await _controller.Delete(employeeId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Сотрудник id:{employeeId} не найден", notFoundResult.Value);
        }



        #endregion

        #region Get

        [Fact]
        public void GetAllEmployyes_ShouldReturnAllEmployees_WhenCalled()
        {
            // Arrange
            Employee leader = new Employee 
            {
                Id = 1, 
                FirstName = "Иван", 
                Patronymic = "Иванович", 
                SurName = "Петров", 
                JobTitle = Position.Director 
            };
            Employee subordinate1 = new Employee 
            { 
                Id = 2, 
                FirstName = "Анна", 
                Patronymic = "Сергеевна", 
                SurName = "Кузнецова",
                JobTitle = Position.Manager,
                Leader = leader 
            };
            Employee subordinate2 = new Employee 
            { 
                Id = 3,
                FirstName = "Олег", 
                Patronymic = "Викторович",
                SurName = "Сидоров",
                JobTitle = Position.TeamLead, 
                Leader = leader 
            };
            leader.Subordinates.Add(subordinate1);
            leader.Subordinates.Add(subordinate2);

            var allEmployees = new List<Employee> { leader, subordinate1, subordinate2 }.AsQueryable();
            _employeesStore.Setup(store => store.GetAll()).Returns(allEmployees);

            // Act
            IActionResult result = _controller.GetAllEmployyes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEmployees = Assert.IsAssignableFrom<IEnumerable<EmployeeViewModel>>(okResult.Value);
            Assert.Equal(allEmployees.Count(), returnedEmployees.Count());
            Assert.NotNull(returnedEmployees.FirstOrDefault(e => e.Id == leader.Id));
            Assert.NotNull(returnedEmployees.FirstOrDefault(e => e.Id == subordinate1.Id));
            Assert.NotNull(returnedEmployees.FirstOrDefault(e => e.Id == subordinate2.Id));
        }

        [Fact]
        public void GetAllEmployed_ShouldReturnOnlyActiveEmployees_WhenCalled()
        {
            // Arrange
            Employee leader = new Employee
            {
                Id = 1,
                FirstName = "Иван",
                Patronymic = "Иванович",
                SurName = "Петров",
                JobTitle = Position.Director
            };
            Employee subordinate1 = new Employee
            {
                Id = 2,
                FirstName = "Анна",
                Patronymic = "Сергеевна",
                SurName = "Кузнецова",
                JobTitle = Position.Manager,
                Leader = leader
            };
            Employee subordinate2 = new Employee
            {
                Id = 3,
                FirstName = "Олег",
                Patronymic = "Викторович",
                SurName = "Сидоров",
                JobTitle = Position.TeamLead,
                Leader = leader,
                IsEmployed = false
            };
            leader.Subordinates.Add(subordinate1);
            leader.Subordinates.Add(subordinate2);

            var allEmployees = new List<Employee> { leader, subordinate1, subordinate2 }.AsQueryable();
            var allEmployedEmployees = new List<Employee> { leader, subordinate1 }.AsQueryable();
            _employeesStore.Setup(store => store.GetAll()).Returns(allEmployees);
            _employeesStore.Setup(store => store.GetAllEmployed()).Returns(allEmployedEmployees);
            // Act
            IActionResult result = _controller.GetAllEmployedEmployyes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEmployees = Assert.IsAssignableFrom<IEnumerable<EmployeeViewModel>>(okResult.Value);
            Assert.Equal(allEmployedEmployees.Count(), returnedEmployees.Count());
            Assert.NotNull(returnedEmployees.FirstOrDefault(e => e.Id == leader.Id));
            Assert.NotNull(returnedEmployees.FirstOrDefault(e => e.Id == subordinate1.Id));
            Assert.Null(returnedEmployees.FirstOrDefault(e => e.Id == subordinate2.Id));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Get_ShouldReturnEmployee_WhenEmployeeExists(int employeeId)
        {
            // Arrange
            var expectedEmployee = new Employee { Id = employeeId, FirstName = "Иван", Patronymic = "Иванович", SurName = "Иванов", JobTitle = Position.Director };
            _employeesStore.Setup(store => store.GetById(employeeId)).Returns(expectedEmployee);

            // Act
            IActionResult result = _controller.Get(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var employeeVM = Assert.IsType<EmployeeViewModel>(okResult.Value);
            Assert.Equal(expectedEmployee.Id, employeeVM.Id);
            Assert.Equal(expectedEmployee.FirstName, employeeVM.FirstName);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(0)]
        [InlineData(-1)]
        public void Get_ShouldReturnNotFound_WhenEmployeeDoesNotExist(int employeeId)
        {
            // Arrange
            _employeesStore.Setup(store => store.GetById(employeeId)).Returns((Employee?)null);

            // Act
            IActionResult result = _controller.Get(employeeId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Сотрудник id:{employeeId} не найден", notFoundResult.Value);
        }

        #endregion

        #region SetTestData

        [Theory]
        [InlineData(-10)]
        [InlineData(124332)]
        public async Task SetTestData_ShouldReturnBadRequest_WhenCountIsOutOfRange(int outOfRangeCount)
        {
            // Arrange

            // Act
            IActionResult result = await _controller.SetTestData(outOfRangeCount);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Число {outOfRangeCount} должно быть в промежутке [{MIN_COUNT}:{MAX_COUNT}]", badRequestResult.Value);
        }



        #endregion

    }
}
