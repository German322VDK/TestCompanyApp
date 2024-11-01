using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using TestCompanyApp.Database.Context;
using TestCompanyApp.Domain.Entity;
using TestCompanyApp.Domain.ViewModels;
using TestCompanyApp.Infrastructure.Services.Mapping;

namespace TestCompanyApp.Infrastructure.Services.Stores.Employees
{
    /// <summary>
    /// Класс для работы с хранилищем сотрудников в базе данных.
    /// </summary>
    public class EmployeesDBStore : IEmployeesStore
    {
        private readonly TestCompanyAppDbContext _dbContext;
        private readonly ILogger<EmployeesDBStore> _logger;

        /// <summary>
        /// Конструктор для инициализации контекста базы данных и логгера.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных.</param>
        /// <param name="logger">Логгер для записи информации и ошибок.</param>
        public EmployeesDBStore(TestCompanyAppDbContext dbContext, ILogger<EmployeesDBStore> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Employee> AddAsync(AddedEmployeeViewModel employeeVM)
        {
            Employee? employee = employeeVM.ToEntity();
            employee = CheckAddedEmployee(employee);

            EntityEntry<Employee> result;
            result = await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            return result.Entity;
        }

        /// <inheritdoc/>
        public async Task<IQueryable<Employee>> AddRangeAsync(IEnumerable<AddedEmployeeViewModel> employeesVM)
        {
            var employees = employeesVM.ToEntity().ToArray();

            for (int i = 0; i < employees.Length; i++)
            {
                employees[i] = CheckAddedEmployee(employees[i]);
            }

            await _dbContext.Employees.AddRangeAsync(employees);
            await _dbContext.SaveChangesAsync();

            return employees.AsQueryable();
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            if (!TryGetEmployeeByIdWithTracking(id, out Employee? employee))
            {
                return false;
            }

            if (employee!.IsEmployed)
            {
                _logger.LogWarning($"Нельзя удалить сотрудника id:{id}, так как он не уволен");
                return false;
            }

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Сотрудник id:{id} успешно удалён из бд");
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAllAsync()
        {
            _dbContext.Employees.RemoveRange(GetAll());
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Сотрудники успешно удалёны из бд");
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> FireAsync(int id, int? newLeaderId = null)
        {
            if (!TryGetEmployeeByIdWithTracking(id, out Employee? employee))
            {
                return false;
            }

            if (!employee!.IsEmployed)
            {
                _logger.LogWarning($"Нельзя уволить сотрудника id:{id}, так как он уволен");
                return false;
            }

            if(HasSubordinates(id) ?? false)
            {
                if(newLeaderId == null)
                {
                    _logger.LogWarning($"У сотрудника id:{id} есть подчинённые но нет нового руководителя");
                    return false;
                }
                else
                {
                    if (!TryGetEmployeeById(newLeaderId.Value, out _))
                    {
                        return false;
                    }

                    _logger.LogInformation($"Подчинённые сотрудника id:{id} переписаны на руководителя id:{newLeaderId}");
                    await SetNewLeaderAsync(id, newLeaderId.Value);
                }
            }

            employee.IsEmployed = false;
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Сотрудник id:{id} успешно уволен");
            return true;
        }

        /// <inheritdoc/>
        public IQueryable<Employee> GetAll() =>
            GetAllWithTracking().AsNoTracking();

        /// <summary>
        /// Получает всех сотрудников с отслеживанием изменений.
        /// </summary>
        /// <returns>Список всех сотрудников с отслеживанием.</returns>
        private IQueryable<Employee> GetAllWithTracking() =>
            _dbContext.Employees;

        /// <inheritdoc/>
        public IQueryable<Employee> GetAllEmployed() =>
            GetAll().Where(emp => emp.IsEmployed);

        /// <summary>
        /// Получает всех действующих сотрудников с отслеживанием.
        /// </summary>
        /// <returns></returns>
        private IQueryable<Employee> GetAllEmployedlWithTracking() =>
            GetAllWithTracking().Where(emp => emp.IsEmployed);

        /// <inheritdoc/>
        public IQueryable<Employee> GetAllSubordinates(int leaderId) =>
            GetAllEmployed().Where(emp => emp.LeaderId == leaderId);

        /// <summary>
        /// Получает всех подчинённых с отслеживанием изменений.
        /// </summary>
        /// <param name="leaderId">Идентификатор руководителя.</param>
        /// <returns>Список подчинённых с отслеживанием.</returns>
        private IQueryable<Employee> GetAllSubordinatesWithTracking(int leaderId) =>
            GetAllEmployedlWithTracking().Where(emp => emp.LeaderId == leaderId);

        /// <inheritdoc/>
        public Employee? GetById(int id) =>
            GetAll().FirstOrDefault(emp => emp.Id == id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Employee? GetByIdWithTracking(int id) =>
            GetAllWithTracking().FirstOrDefault(emp => emp.Id == id);

        ///<inheritdoc/>
        public bool? HasSubordinates(int id) =>
            GetById(id)?.Subordinates?.Any();

        ///<inheritdoc/>
        public bool? IsEmployed(int id) =>
            GetById(id)?.IsEmployed;

        /// <inheritdoc/>
        public async Task<bool> SetLeaderAsync(int subordinateId, int newLeaderId)
        {
            if (!TryGetEmployeeByIdWithTracking(subordinateId, out Employee? subordinate))
            {
                return false;
            }

            if (!TryGetEmployeeById(newLeaderId, out var newLeader))
            {
                return false;
            }

            if(subordinate!.LeaderId != null)
            {
                if (!TryGetEmployeeById(subordinate.LeaderId.Value, out Employee? oldLeader))
                {
                    return false;
                }

                if (!oldLeader!.Subordinates!.Any(emp => emp.Id == newLeaderId))
                {
                    _logger.LogWarning($"Сотрудник id:{newLeaderId} не является непосредственным подчинённым " +
                        $"сотрудника id:{oldLeader.Id}");
                    return false;
                }
            }
            subordinate!.LeaderId = newLeaderId;
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Подчинённый сотрудник id:{subordinateId} стал подчинённым сотрудника id:{newLeaderId}");

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> SetNewLeaderAsync(int oldLeaderId, int newLeaderId)
        {
            if (!TryGetEmployeeById(oldLeaderId, out var oldLeader))
            {
                return false;
            }

            if (!TryGetEmployeeByIdWithTracking(newLeaderId, out var newLeader))
            {
                return false;
            }

            if(!oldLeader!.Subordinates!.Any(emp => emp.Id == newLeaderId))
            {
                _logger.LogWarning($"Сотрудник id:{newLeaderId} не является непосредственным подчинённым " +
                    $"сотрудника id:{oldLeaderId}");
                return false;
            }

            var employees = GetAllSubordinatesWithTracking(oldLeaderId);
            foreach (var employee in employees)
            {
                employee.LeaderId = newLeaderId;
            }

            //повышение
            newLeader!.LeaderId = oldLeader.LeaderId;
            newLeader.JobTitle = oldLeader.JobTitle;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Все подчинённые сотрудника id:{oldLeaderId} стали подчинёнными сотрудника id:{newLeaderId}");

            return true;
        }

        private Employee CheckAddedEmployee(Employee? employee)
        {
            if (employee == null)
            {
                string errorEmployeeNullMessage = $"Ошибка преобразования. Модель представления сотрудника не удалось " +
                    $"преобразовать в сущность Employee.";
                _logger.LogError(errorEmployeeNullMessage);
                throw new ArgumentException(errorEmployeeNullMessage);
            }

            if (employee.LeaderId == null || GetByIdWithTracking(employee.LeaderId!.Value) == null || employee.JobTitle == Position.Director)
            {
                var employedEmployees = GetAllEmployed();
                if (employedEmployees.Any(emp => emp.LeaderId == null) || 
                    employedEmployees.Any(emp => emp.JobTitle == Position.Director))
                {
                    string errorEmployeeDirectorMessage = $"Добавление невозможно: сотрудник с должностью 'Директор' " +
                        $"или без руководителя уже существует в системе.";
                    _logger.LogError(errorEmployeeDirectorMessage);
                    throw new ArgumentException(errorEmployeeDirectorMessage);
                }

                return employee;
            }

            return employee;
        }

        private bool TryGetEmployeeById(int id, out Employee? employee)
        {
            employee = GetById(id);
            return TryGetEmployee(id, employee);
        }

        private bool TryGetEmployeeByIdWithTracking(int id, out Employee? employee)
        {
            employee = GetByIdWithTracking(id);
            return TryGetEmployee(id, employee);
        }

        private bool TryGetEmployee(int id, Employee? employee)
        {
            if (employee == null)
            {
                _logger.LogWarning($"Сотрудник id:{id} не найден");
                return false;
            }
            return true;
        }

    }
}
