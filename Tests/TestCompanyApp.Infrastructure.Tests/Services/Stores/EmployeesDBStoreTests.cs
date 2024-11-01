using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TestCompanyApp.Database.Context;
using TestCompanyApp.Domain.Entity;
using TestCompanyApp.Domain.ViewModels;
using TestCompanyApp.Infrastructure.Services.Stores.Employees;

namespace TestCompanyApp.Infrastructure.Tests.Services.Stores
{
    public class EmployeesDBStoreTests
    {
        private readonly DbContextOptions<TestCompanyAppDbContext> _options;
        private readonly Mock<ILogger<EmployeesDBStore>> _mockLogger;

        private Employee _director;
        private Employee _manager;

        public EmployeesDBStoreTests()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            _options = new DbContextOptionsBuilder<TestCompanyAppDbContext>()
                .UseSqlite(connection)
                .UseLazyLoadingProxies()
                .Options;

            _mockLogger = new Mock<ILogger<EmployeesDBStore>>();

            Init();
        }

        private async void Init()
        {
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);

            _director = await employeesDBStore.AddAsync(new AddedEmployeeViewModel
            {
                JobTitle = Position.Director,
                FirstName = "Иван",
                Patronymic = "Петрович",
                SurName = "Васильев"
            });

            _manager = await employeesDBStore.AddAsync(new AddedEmployeeViewModel
            {
                JobTitle = Position.Manager,
                LeaderId = _director.Id,
                FirstName = "Василий",
                Patronymic = "Иванович",
                SurName = "Петровов"
            });
        }

        #region StaticData

        public static IEnumerable<object[]> GetCorrectEmployeeData()
        {
            yield return new List<AddedEmployeeViewModel>[] { new List<AddedEmployeeViewModel>
            {
                new AddedEmployeeViewModel { FirstName = "Иван", Patronymic = "Иванович", SurName = "Петров", JobTitle = Position.Manager, LeaderId = 1 },
                new AddedEmployeeViewModel { FirstName = "Мария", Patronymic = "Сергеевна", SurName = "Кузнецова", JobTitle = Position.TeamLead, LeaderId = 1 },
                new AddedEmployeeViewModel { FirstName = "Антон", Patronymic = "Андреевич", SurName = "Сидоров", JobTitle = Position.Developer, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Елена", Patronymic = "Алексеевна", SurName = "Смирнова", JobTitle = Position.QAEngineer, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Николай", Patronymic = "Павлович", SurName = "Федоров", JobTitle = Position.Designer, LeaderId = 1 },
                new AddedEmployeeViewModel { FirstName = "Дарья", Patronymic = "Игоревна", SurName = "Новикова", JobTitle = Position.Analyst, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Петр", Patronymic = "Петрович", SurName = "Морозов", JobTitle = Position.Administrator, LeaderId = 1 },
                new AddedEmployeeViewModel { FirstName = "Светлана", Patronymic = "Михайловна", SurName = "Горбунова", JobTitle = Position.SupportSpecialist, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Алексей", Patronymic = "Викторович", SurName = "Соколов", JobTitle = Position.Manager, LeaderId = 1 },
                new AddedEmployeeViewModel { FirstName = "Ольга", Patronymic = "Юрьевна", SurName = "Романова", JobTitle = Position.SupportSpecialist, LeaderId = 2 }
            }};

            yield return new List<AddedEmployeeViewModel>[] { new List<AddedEmployeeViewModel>
            {
                new AddedEmployeeViewModel { FirstName = "Виктор", Patronymic = "Сергеевич", SurName = "Громов", JobTitle = Position.Analyst, LeaderId = 1 },
                new AddedEmployeeViewModel { FirstName = "Анастасия", Patronymic = "Игоревна", SurName = "Павлова", JobTitle = Position.TeamLead, LeaderId = 1 },
                new AddedEmployeeViewModel { FirstName = "Сергей", Patronymic = "Александрович", SurName = "Петров", JobTitle = Position.Developer, LeaderId = 2 }
            }};

            yield return new List<AddedEmployeeViewModel>[] { new List<AddedEmployeeViewModel>
            {
                new AddedEmployeeViewModel { FirstName = "Мария", Patronymic = "Станиславовна", SurName = "Коваль", JobTitle = Position.QAEngineer, LeaderId = 1 },
                new AddedEmployeeViewModel { FirstName = "Дмитрий", Patronymic = "Иванович", SurName = "Семенов", JobTitle = Position.Designer, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Екатерина", Patronymic = "Сергеевна", SurName = "Фролова", JobTitle = Position.Manager, LeaderId = 1 },
                new AddedEmployeeViewModel { FirstName = "Андрей", Patronymic = "Олегович", SurName = "Сердюков", JobTitle = Position.SupportSpecialist, LeaderId = 2 }
            }};
        }

        public static IEnumerable<object[]> GetCorrectSubordinatesData()
        {
            yield return new List<AddedEmployeeViewModel>[] { new List<AddedEmployeeViewModel>
            {
                new AddedEmployeeViewModel { FirstName = "Иван", Patronymic = "Иванович", SurName = "Петров", JobTitle = Position.Manager, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Мария", Patronymic = "Сергеевна", SurName = "Кузнецова", JobTitle = Position.TeamLead, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Антон", Patronymic = "Андреевич", SurName = "Сидоров", JobTitle = Position.Developer, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Елена", Patronymic = "Алексеевна", SurName = "Смирнова", JobTitle = Position.QAEngineer, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Николай", Patronymic = "Павлович", SurName = "Федоров", JobTitle = Position.Designer, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Дарья", Patronymic = "Игоревна", SurName = "Новикова", JobTitle = Position.Analyst, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Петр", Patronymic = "Петрович", SurName = "Морозов", JobTitle = Position.Administrator, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Светлана", Patronymic = "Михайловна", SurName = "Горбунова", JobTitle = Position.SupportSpecialist, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Алексей", Patronymic = "Викторович", SurName = "Соколов", JobTitle = Position.Manager, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Ольга", Patronymic = "Юрьевна", SurName = "Романова", JobTitle = Position.SupportSpecialist, LeaderId = 2 }
            }};

            yield return new List<AddedEmployeeViewModel>[] { new List<AddedEmployeeViewModel>
            {
                new AddedEmployeeViewModel { FirstName = "Виктор", Patronymic = "Сергеевич", SurName = "Громов", JobTitle = Position.Analyst, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Анастасия", Patronymic = "Игоревна", SurName = "Павлова", JobTitle = Position.TeamLead, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Сергей", Patronymic = "Александрович", SurName = "Петров", JobTitle = Position.Developer, LeaderId = 2 }
            }};

            yield return new List<AddedEmployeeViewModel>[] { new List<AddedEmployeeViewModel>
            {
                new AddedEmployeeViewModel { FirstName = "Мария", Patronymic = "Станиславовна", SurName = "Коваль", JobTitle = Position.QAEngineer, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Дмитрий", Patronymic = "Иванович", SurName = "Семенов", JobTitle = Position.Designer, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Екатерина", Patronymic = "Сергеевна", SurName = "Фролова", JobTitle = Position.Manager, LeaderId = 2 },
                new AddedEmployeeViewModel { FirstName = "Андрей", Patronymic = "Олегович", SurName = "Сердюков", JobTitle = Position.SupportSpecialist, LeaderId = 2 }
            }};
        }

        #endregion

        #region Add

        [Theory]
        [InlineData("Иван", "Иванович", "Петров", Position.Manager, 1)]
        [InlineData("Мария", "Сергеевна", "Кузнецова", Position.TeamLead, 1)]
        [InlineData("Антон", "Андреевич", "Сидоров", Position.Developer, 2)]
        [InlineData("Елена", "Алексеевна", "Смирнова", Position.QAEngineer, 2)]
        [InlineData("Николай", "Павлович", "Федоров", Position.Designer, 1)]
        [InlineData("Дарья", "Игоревна", "Новикова", Position.Analyst, 2)]
        [InlineData("Петр", "Петрович", "Морозов", Position.Administrator, 1)]
        [InlineData("Светлана", "Михайловна", "Горбунова", Position.SupportSpecialist, 2)]
        [InlineData("Алексей", "Викторович", "Соколов", Position.Manager, 1)]
        [InlineData("Ольга", "Юрьевна", "Романова", Position.SupportSpecialist, 2)]
        private async Task AddEmployee_ShouldAddEmployeeToDatabase(string firstName, string patronymic,
            string surName, Position jobTitle, int leaderId)
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);

            AddedEmployeeViewModel employeeViewModel = new AddedEmployeeViewModel
            {
                FirstName = firstName,
                Patronymic = patronymic,
                SurName = surName,
                JobTitle = jobTitle,
                LeaderId = leaderId
            };
            Employee? leader = employeesDBStore.GetById(leaderId);

            // Act
            Employee addedEmployee = await employeesDBStore.AddAsync(employeeViewModel);

            // Assert
            Assert.NotNull(leader);
            Assert.NotNull(addedEmployee);
            Assert.Equal(firstName, addedEmployee.FirstName);
            Assert.Equal(patronymic, addedEmployee.Patronymic);
            Assert.Equal(surName, addedEmployee.SurName);
            Assert.Equal(leaderId, addedEmployee.LeaderId);
            Assert.Equal(jobTitle, addedEmployee.JobTitle);
            Assert.Equal(addedEmployee.LeaderId, leaderId);
            Assert.NotNull(addedEmployee.Leader);
            Assert.True(addedEmployee.IsEmployed);
            Assert.Contains(addedEmployee.Id, leader.Subordinates.Select(sub => sub.Id));
        }

        [Theory]
        [InlineData("Антон", "Викторович", "Сидоров", Position.Director, 0)]
        [InlineData("Ольга", "Алексеевна", "Кузнецова", Position.SupportSpecialist, -1)]
        private async Task AddEmployee_ShouldThrowException_WhenAddingSecondDirector(string firstName, string patronymic,
            string surName, Position jobTitle, int leaderId)
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);

            AddedEmployeeViewModel employeeViewModel = new AddedEmployeeViewModel
            {
                FirstName = firstName,
                Patronymic = patronymic,
                SurName = surName,
                JobTitle = jobTitle,
                LeaderId = leaderId == -1 ? null : leaderId
            };

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => employeesDBStore.AddAsync(employeeViewModel));
            // Assert
            Assert.Equal($"Добавление невозможно: сотрудник с должностью 'Директор' " +
                        $"или без руководителя уже существует в системе.", exception.Message);
        }

        [Fact]
        private async Task AddEmployee_ShouldThrowException_WhenAddingNullEmployee()
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);
            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => employeesDBStore.AddAsync(null));
            // Assert
            Assert.Equal($"Ошибка преобразования. Модель представления сотрудника не удалось " +
                    $"преобразовать в сущность Employee.", exception.Message);
        }


        #endregion

        #region AddRange

        [Theory]
        [MemberData(nameof(GetCorrectEmployeeData))]
        private async Task AddEmployees_ShouldAddEmployeesToDatabase(List<AddedEmployeeViewModel> employees)
        {
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);

            // Act
            var addedEmployeesArray = (await employeesDBStore.AddRangeAsync(employees)).ToArray();

            // Assert
            Assert.Equal(addedEmployeesArray.Length, employees.Count);
            for (int i = 0; i < addedEmployeesArray.Length; i++)
            {
                var leader = addedEmployeesArray[i].Leader;
                Assert.NotNull(leader);
                Assert.NotNull(addedEmployeesArray[i]);
                Assert.Equal(employees[i].FirstName, addedEmployeesArray[i].FirstName);
                Assert.Equal(employees[i].Patronymic, addedEmployeesArray[i].Patronymic);
                Assert.Equal(employees[i].SurName, addedEmployeesArray[i].SurName);
                Assert.Equal(employees[i].LeaderId, addedEmployeesArray[i].LeaderId);
                Assert.Equal(employees[i].JobTitle, addedEmployeesArray[i].JobTitle);
                Assert.Equal(employees[i].LeaderId, addedEmployeesArray[i].LeaderId);
                Assert.NotNull(addedEmployeesArray[i].Leader);
                Assert.True(addedEmployeesArray[i].IsEmployed);
                Assert.Contains(addedEmployeesArray[i].Id, leader.Subordinates.Select(sub => sub.Id));
            }


        }

        [Theory]
        [InlineData("Антон", "Викторович", "Сидоров", Position.Director, 0)]
        [InlineData("Ольга", "Алексеевна", "Кузнецова", Position.SupportSpecialist, -1)]
        private async Task AddEmployees_ShouldThrowException_WhenAddingSecondDirector(string firstName, string patronymic,
           string surName, Position jobTitle, int leaderId)
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);

            AddedEmployeeViewModel employeeViewModel = new AddedEmployeeViewModel
            {
                FirstName = firstName,
                Patronymic = patronymic,
                SurName = surName,
                JobTitle = jobTitle,
                LeaderId = leaderId == -1 ? null : leaderId
            };

            IEnumerable<AddedEmployeeViewModel> employeeViewModels = new List<AddedEmployeeViewModel>()
            {
                employeeViewModel
            };

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => employeesDBStore.AddRangeAsync(employeeViewModels));
            // Assert
            Assert.Equal($"Добавление невозможно: сотрудник с должностью 'Директор' " +
                        $"или без руководителя уже существует в системе.", exception.Message);
        }


        #endregion

        #region Delete

        [Theory]
        [InlineData("Иван", "Иванович", "Петров", Position.Manager, 1)]
        [InlineData("Мария", "Сергеевна", "Кузнецова", Position.TeamLead, 1)]
        [InlineData("Антон", "Андреевич", "Сидоров", Position.Developer, 2)]
        [InlineData("Елена", "Алексеевна", "Смирнова", Position.QAEngineer, 2)]
        [InlineData("Николай", "Павлович", "Федоров", Position.Designer, 1)]
        [InlineData("Дарья", "Игоревна", "Новикова", Position.Analyst, 2)]
        [InlineData("Петр", "Петрович", "Морозов", Position.Administrator, 1)]
        [InlineData("Светлана", "Михайловна", "Горбунова", Position.SupportSpecialist, 2)]
        [InlineData("Алексей", "Викторович", "Соколов", Position.Manager, 1)]
        [InlineData("Ольга", "Юрьевна", "Романова", Position.SupportSpecialist, 2)]
        private async Task DeleteEmployee_ShouldDeleteEmployeeFromDatabase(string firstName, string patronymic,
            string surName, Position jobTitle, int leaderId)
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);

            AddedEmployeeViewModel employeeViewModel = new AddedEmployeeViewModel
            {
                FirstName = firstName,
                Patronymic = patronymic,
                SurName = surName,
                JobTitle = jobTitle,
                LeaderId = leaderId
            };
            Employee addedEmployee = await employeesDBStore.AddAsync(employeeViewModel);
            bool fireResult = await employeesDBStore.FireAsync(addedEmployee.Id);

            // Act
            bool deletedResult = await employeesDBStore.DeleteAsync(addedEmployee.Id);

            // Assert
            Employee? deletedEmployee = employeesDBStore.GetById(addedEmployee.Id);
            Assert.True(fireResult);
            Assert.True(deletedResult);
            Assert.Null(deletedEmployee);
        }

        [Theory]
        [InlineData("Иван", "Иванович", "Петров", Position.Manager, 1)]
        [InlineData("Мария", "Сергеевна", "Кузнецова", Position.TeamLead, 1)]
        [InlineData("Антон", "Андреевич", "Сидоров", Position.Developer, 2)]
        [InlineData("Елена", "Алексеевна", "Смирнова", Position.QAEngineer, 2)]
        [InlineData("Николай", "Павлович", "Федоров", Position.Designer, 1)]
        [InlineData("Дарья", "Игоревна", "Новикова", Position.Analyst, 2)]
        [InlineData("Петр", "Петрович", "Морозов", Position.Administrator, 1)]
        [InlineData("Светлана", "Михайловна", "Горбунова", Position.SupportSpecialist, 2)]
        [InlineData("Алексей", "Викторович", "Соколов", Position.Manager, 1)]
        [InlineData("Ольга", "Юрьевна", "Романова", Position.SupportSpecialist, 2)]
        private async Task DeleteNotFiredEmployee_ShouldNotDeleteEmployeeFromDatabase(string firstName, string patronymic,
            string surName, Position jobTitle, int leaderId)
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);

            AddedEmployeeViewModel employeeViewModel = new AddedEmployeeViewModel
            {
                FirstName = firstName,
                Patronymic = patronymic,
                SurName = surName,
                JobTitle = jobTitle,
                LeaderId = leaderId
            };
            Employee addedEmployee = await employeesDBStore.AddAsync(employeeViewModel);

            // Act
            bool deletedResult = await employeesDBStore.DeleteAsync(addedEmployee.Id);

            // Assert
            Employee? deletedEmployee = employeesDBStore.GetById(addedEmployee.Id);
            Assert.False(deletedResult);
            Assert.NotNull(deletedEmployee);
        }

        [Fact]
        private async Task DeleteNotExistingEmployee_ShouldNotDeleteEmployeeFromDatabase()
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);

            // Act
            bool deletedResult = await employeesDBStore.DeleteAsync(-1);

            // Assert
            Assert.False(deletedResult);
        }

        #endregion

        #region Fire

        [Theory]
        [InlineData("Иван", "Иванович", "Петров", Position.Manager, 1)]
        [InlineData("Мария", "Сергеевна", "Кузнецова", Position.TeamLead, 1)]
        [InlineData("Антон", "Андреевич", "Сидоров", Position.Developer, 2)]
        [InlineData("Елена", "Алексеевна", "Смирнова", Position.QAEngineer, 2)]
        [InlineData("Николай", "Павлович", "Федоров", Position.Designer, 1)]
        [InlineData("Дарья", "Игоревна", "Новикова", Position.Analyst, 2)]
        [InlineData("Петр", "Петрович", "Морозов", Position.Administrator, 1)]
        [InlineData("Светлана", "Михайловна", "Горбунова", Position.SupportSpecialist, 2)]
        [InlineData("Алексей", "Викторович", "Соколов", Position.Manager, 1)]
        [InlineData("Ольга", "Юрьевна", "Романова", Position.SupportSpecialist, 2)]
        private async Task FireEmployeeWithoutSubordinates_ShouldFireEmployeeToDatabase(string firstName, string patronymic,
            string surName, Position jobTitle, int leaderId)
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);

            AddedEmployeeViewModel employeeViewModel = new AddedEmployeeViewModel
            {
                FirstName = firstName,
                Patronymic = patronymic,
                SurName = surName,
                JobTitle = jobTitle,
                LeaderId = leaderId
            };
            Employee addedEmployee = await employeesDBStore.AddAsync(employeeViewModel);

            // Act
            bool result = await employeesDBStore.FireAsync(addedEmployee.Id);

            // Assert
            Assert.True(result);
            Assert.False(employeesDBStore.IsEmployed(addedEmployee.Id));
            Assert.DoesNotContain(addedEmployee.Id, employeesDBStore.GetAllEmployed().Select(emp => emp.Id));
        }

        [Theory]
        [MemberData(nameof(GetCorrectSubordinatesData))]
        private async Task FireEmployeeWithSubordinates_ShouldFireEmployeeToDatabase(List<AddedEmployeeViewModel> employees)
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);
            var addedEmployees = (await employeesDBStore.AddRangeAsync(employees)).ToList();
            var newLeader = addedEmployees[0];

            // Act
            bool result = await employeesDBStore.FireAsync(_manager.Id, newLeader.Id);

            // Assert
            Employee? newUpdatedLeader = employeesDBStore.GetById(newLeader.Id);
            Assert.True(result);
            Assert.False(employeesDBStore.IsEmployed(_manager.Id));
            Assert.DoesNotContain(_manager.Id, employeesDBStore.GetAllEmployed().Select(emp => emp.Id));
            Assert.NotNull(newUpdatedLeader);
            Assert.Equal(addedEmployees.Count - 1, newUpdatedLeader.Subordinates.Count);
            foreach (var employee in addedEmployees)
            {
                if(employee.Id != newUpdatedLeader.Id)
                {
                    Assert.Contains(employee.Id, newUpdatedLeader.Subordinates.Select(emp => emp.Id));
                }
            }
        }

        #endregion

        #region Get

        [Fact]
        private void GetAllShouldReturnEmployeesFromDatabase()
        {
            // Arrange
            const int COUNT = 2;
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);

            // Act
            var employees = employeesDBStore.GetAll();

            // Assert
            Assert.NotNull(employees);
            Assert.Equal(COUNT, employees.Count());
            Assert.True(employees.Any(emp => emp.Id == _director.Id));
            Assert.True(employees.Any(emp => emp.Id == _manager.Id));
            Assert.False(employees.Any(emp => emp.Id == 3));
        }

        [Fact]
        private void GetByIdShouldReturnEmployeeFromDatabase()
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);

            // Act
            Employee? director = employeesDBStore.GetById(_director.Id);
            Employee? manager = employeesDBStore.GetById(_manager.Id);
            Employee? notExistingEmlpoyee = employeesDBStore.GetById(-1);
            // Assert
            Assert.NotNull(director);
            Assert.NotNull(manager);
            Assert.Null(notExistingEmlpoyee);
            Assert.Equal(_director.Id, director.Id);
            Assert.Equal(_manager.Id, manager.Id);
        }

        [Fact]
        private async Task GetAllEmployedShouldReturnEmployeesEmployedFromDatabase()
        {
            // Arrange
            const int COUNT_EMPLOYED = 1;
            const int COUNT_ALL = 2;
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);
            bool result = await employeesDBStore.FireAsync(_manager.Id);

            // Act
            var employeedEmployees = employeesDBStore.GetAllEmployed();
            var allEmployees = employeesDBStore.GetAll();

            // Assert
            Assert.NotNull(employeedEmployees);
            Assert.NotNull(allEmployees);
            Assert.Equal(COUNT_EMPLOYED, employeedEmployees.Count());
            Assert.Equal(COUNT_ALL, allEmployees.Count());
            Assert.True(employeedEmployees.Any(emp => emp.Id == _director.Id));
            Assert.False(employeedEmployees.Any(emp => emp.Id == _manager.Id));
            Assert.True(allEmployees.Any(emp => emp.Id == _director.Id));
            Assert.True(allEmployees.Any(emp => emp.Id == _manager.Id));
            Assert.True(result);
        }

        [Fact]
        private void GetAllSubordinatesShouldReturnEmployeesSubordinatesFromDatabase()
        {
            // Arrange
            const int COUNT_DIRECTORS = 1;
            const int COUNT_MANAGER = 0;
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);

            // Act
            var directorsSubordinates = employeesDBStore.GetAllSubordinates(_director.Id);
            var managersSubordinates = employeesDBStore.GetAllSubordinates(_manager.Id);

            // Assert
            Assert.NotNull(directorsSubordinates);
            Assert.NotNull(managersSubordinates);
            Assert.Equal(COUNT_DIRECTORS, directorsSubordinates.Count());
            Assert.Equal(COUNT_MANAGER, managersSubordinates.Count());
        }

        #endregion

        #region HasSubordinates

        [Fact]
        private void HasSubordinatesShouldReturnResult()
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);

            // Act
            bool? directorResult = employeesDBStore.HasSubordinates(_director.Id);
            bool? managerResult = employeesDBStore.HasSubordinates(_manager.Id);
            bool? notExistingResult = employeesDBStore.HasSubordinates(-1);

            // Assert
            Assert.NotNull(directorResult);
            Assert.NotNull(managerResult);
            Assert.Null(notExistingResult);
            Assert.True(directorResult);
            Assert.False(managerResult);
        }

        #endregion

        #region IsEmployed

        [Fact]
        private async Task IsEmployed_ShouldReturnEmploymentStatus_WhenCheckingEmployeeStatus()
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);
            bool result = await employeesDBStore.FireAsync(_manager.Id);

            // Act
            bool? directorResult = employeesDBStore.IsEmployed(_director.Id);
            bool? managerResult = employeesDBStore.IsEmployed(_manager.Id);
            bool? notExistingResult = employeesDBStore.IsEmployed(-1);

            // Assert
            Assert.NotNull(directorResult);
            Assert.NotNull(managerResult);
            Assert.Null(notExistingResult);
            Assert.True(directorResult);
            Assert.False(managerResult);
        }

        #endregion

        #region SetLeaderAsync

        [Theory]
        [InlineData("Иван", "Иванович", "Петров", Position.Manager, 1)]
        [InlineData("Мария", "Сергеевна", "Кузнецова", Position.TeamLead, 1)]
        [InlineData("Антон", "Андреевич", "Сидоров", Position.Developer, 2)]
        [InlineData("Елена", "Алексеевна", "Смирнова", Position.QAEngineer, 2)]
        [InlineData("Николай", "Павлович", "Федоров", Position.Designer, 1)]
        [InlineData("Дарья", "Игоревна", "Новикова", Position.Analyst, 2)]
        [InlineData("Петр", "Петрович", "Морозов", Position.Administrator, 1)]
        [InlineData("Светлана", "Михайловна", "Горбунова", Position.SupportSpecialist, 2)]
        [InlineData("Алексей", "Викторович", "Соколов", Position.Manager, 1)]
        [InlineData("Ольга", "Юрьевна", "Романова", Position.SupportSpecialist, 2)]
        private async Task SetLeader_ShouldSetLeaderToDatabase(string firstName, string patronymic,
            string surName, Position jobTitle, int leaderId)
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);

            AddedEmployeeViewModel employeeViewModel1 = new AddedEmployeeViewModel
            {
                FirstName = firstName,
                Patronymic = patronymic,
                SurName = surName,
                JobTitle = jobTitle,
                LeaderId = leaderId
            };
            AddedEmployeeViewModel employeeViewModel2 = new AddedEmployeeViewModel
            {
                FirstName = "firstName",
                Patronymic = "patronymic",
                SurName = "surName",
                JobTitle = jobTitle,
                LeaderId = leaderId
            };

            Employee? leader = employeesDBStore.GetById(leaderId);
            Employee addedEmployee1 = await employeesDBStore.AddAsync(employeeViewModel1);
            Employee addedEmployee2 = await employeesDBStore.AddAsync(employeeViewModel2);
            // Act

            bool result = await employeesDBStore.SetLeaderAsync(addedEmployee1.Id, addedEmployee2.Id);

            // Assert

            Assert.True(result);
            Assert.Equal(addedEmployee1.LeaderId, addedEmployee2.Id);
        }

        [Theory]
        [InlineData("Иван", "Иванович", "Петров", Position.Manager, 1)]
        [InlineData("Мария", "Сергеевна", "Кузнецова", Position.TeamLead, 1)]
        [InlineData("Антон", "Андреевич", "Сидоров", Position.Developer, 2)]
        [InlineData("Елена", "Алексеевна", "Смирнова", Position.QAEngineer, 2)]
        [InlineData("Николай", "Павлович", "Федоров", Position.Designer, 1)]
        [InlineData("Дарья", "Игоревна", "Новикова", Position.Analyst, 2)]
        [InlineData("Петр", "Петрович", "Морозов", Position.Administrator, 1)]
        [InlineData("Светлана", "Михайловна", "Горбунова", Position.SupportSpecialist, 2)]
        [InlineData("Алексей", "Викторович", "Соколов", Position.Manager, 1)]
        [InlineData("Ольга", "Юрьевна", "Романова", Position.SupportSpecialist, 2)]
        private async Task SetNotExistingLeader_ShouldReturnFalse(string firstName, string patronymic,
            string surName, Position jobTitle, int leaderId)
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);
            AddedEmployeeViewModel employeeViewModel1 = new AddedEmployeeViewModel
            {
                FirstName = firstName,
                Patronymic = patronymic,
                SurName = surName,
                JobTitle = jobTitle,
                LeaderId = leaderId
            };
            var addedEmployee = await employeesDBStore.AddAsync(employeeViewModel1);

            //Act
            bool result = await employeesDBStore.SetLeaderAsync(addedEmployee.Id, -1);

            //Assert
            Assert.False(result);
        }

        [Fact]
        private async Task SetLeaderToNotExistingEmployee_ShouldReturnFalse()
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);
            
            //Act
            bool result = await employeesDBStore.SetLeaderAsync(-1, _manager.Id);

            //Assert
            Assert.False(result);
        }


        [Theory]
        [InlineData("Иван", "Иванович", "Петров", Position.Manager, 2)]
        [InlineData("Мария", "Сергеевна", "Кузнецова", Position.TeamLead, 2)]
        [InlineData("Антон", "Андреевич", "Сидоров", Position.Developer, 2)]
        [InlineData("Елена", "Алексеевна", "Смирнова", Position.QAEngineer, 2)]
        [InlineData("Николай", "Павлович", "Федоров", Position.Designer, 2)]
        [InlineData("Дарья", "Игоревна", "Новикова", Position.Analyst, 2)]
        [InlineData("Петр", "Петрович", "Морозов", Position.Administrator, 2)]
        [InlineData("Светлана", "Михайловна", "Горбунова", Position.SupportSpecialist, 2)]
        [InlineData("Алексей", "Викторович", "Соколов", Position.Manager, 2)]
        [InlineData("Ольга", "Юрьевна", "Романова", Position.SupportSpecialist, 2)]
        private async Task SetNotPrimaryLeader_ShouldReturnFalse(string firstName, string patronymic,
            string surName, Position jobTitle, int leaderId)
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);
            AddedEmployeeViewModel employeeViewModel1 = new AddedEmployeeViewModel
            {
                FirstName = firstName,
                Patronymic = patronymic,
                SurName = surName,
                JobTitle = jobTitle,
                LeaderId = leaderId
            };
            var addedEmployee = await employeesDBStore.AddAsync(employeeViewModel1);

            //Act
            bool result = await employeesDBStore.SetLeaderAsync(addedEmployee.Id, _director.Id);

            //Assert
            Assert.False(result);
        }

        #endregion

        #region SetNewLeaderAsync


        [Theory]
        [MemberData(nameof(GetCorrectSubordinatesData))]
        private async Task SetNewLeaderAsync_ShouldSetNewLeaderToDatabase(List<AddedEmployeeViewModel> employees)
        {
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);
            var addedEmployeesArray = (await employeesDBStore.AddRangeAsync(employees)).ToArray();
            var newLeader = addedEmployeesArray[0];

            // Act
            bool result = await employeesDBStore.SetNewLeaderAsync(newLeader.LeaderId.Value, newLeader.Id);

            // Assert
            Employee? newUpdatedLeader = employeesDBStore.GetById(newLeader.Id);
            Assert.True(result);
            Assert.NotNull(newUpdatedLeader);
            foreach (var employee in addedEmployeesArray)
            {
                if (employee.Id != newUpdatedLeader.Id)
                    Assert.Equal(newUpdatedLeader.Id, employee.LeaderId);
            }
            foreach (var employee in newUpdatedLeader.Subordinates)
            {
                Assert.Equal(newUpdatedLeader.Id, employee.LeaderId);
            }
        }

        [Fact]
        private async Task SetNotExistingNewLeaderShouldReturnFalse()
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);

            //Act
            bool result = await employeesDBStore.SetNewLeaderAsync(_director.Id, - 1);

            //Assert
            Assert.False(result);
        }

        [Fact]
        private async Task SetNotExistingOldLeaderShouldReturnFalse()
        {
            // Arrange
            using TestCompanyAppDbContext context = new TestCompanyAppDbContext(_options);
            context.Database.EnsureCreated();
            EmployeesDBStore employeesDBStore = new EmployeesDBStore(context, _mockLogger.Object);

            //Act
            bool result = await employeesDBStore.SetNewLeaderAsync(-1, _director.Id);

            //Assert
            Assert.False(result);
        }

        #endregion
    }
}
 