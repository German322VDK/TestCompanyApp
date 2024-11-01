using Microsoft.AspNetCore.Mvc;
using TestCompanyApp.Domain.Entity;
using TestCompanyApp.Domain.ViewModels;
using TestCompanyApp.Infrastructure.Services.Mapping;
using TestCompanyApp.Infrastructure.Services.Stores.Employees;
using TestCompanyApp.Infrastructure.StaticData;

namespace TestCompanyApp.API.Controllers
{
    /// <summary>
    /// Контроллер для работы с сотрудниками.
    /// </summary>
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesStore _employeesStore;
        private readonly ILogger<EmployeesController> _logger;

        private const int MIN_COUNT = 1;
        private const int MAX_COUNT = 1000;

        /// <summary>
        /// Конструктор контроллера.
        /// </summary>
        /// <param name="employeesStore">Хранилище сотрудников.</param>
        /// <param name="logger">Логгер для контроллера.</param>
        public EmployeesController(IEmployeesStore employeesStore, ILogger<EmployeesController> logger)
        {
            _employeesStore = employeesStore;
            _logger = logger;
        }

        /// <summary>
        /// Добавляет нового сотрудника.
        /// </summary>
        /// <param name="model">Модель нового сотрудника с необходимыми данными.</param>
        /// <returns>Добавленный сотрудник в формате ViewModel.</returns>
        [HttpPost("hire")]
        public async Task<IActionResult> Add(AddedEmployeeViewModel model)
        {
            if(model.LeaderId == null)
            {
                _logger.LogWarning($"Не указан руководитель");
                return BadRequest($"Не указан руководитель");
            }
            Employee addedEmployee;
            try
            {
                addedEmployee = await _employeesStore.AddAsync(model);
            }
            catch(ArgumentException ex)
            {
                _logger.LogWarning($"Ошибка валидации: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch(Exception ex) 
            {
                _logger.LogError($"Ошибка при добавлении сотрудника: {ex.Message}");
                return StatusCode(500, "Произошла ошибка при добавлении сотрудника.");
            }
            
            return Ok(addedEmployee.ToViewModel());
        }

        /// <summary>
        /// Увольняет сотрудника.
        /// </summary>
        /// <param name="employeeId">Идентификатор сотрудника.</param>
        /// <param name="newLeaderId">Идентификатор нового руководителя (может быть null).</param>
        /// <returns>Результат операции увольнения.</returns>
        [HttpPatch("fire")]
        public async Task<IActionResult> Fire(int employeeId, int? newLeaderId)
        {
            Employee? employee = _employeesStore.GetById(employeeId);
            if (employee == null)
            {
                _logger.LogWarning($"Уволняемый сотрудник не найден");
                return NotFound($"Уволняемый сотрудник не найден");
            }

            bool result = await _employeesStore.FireAsync(employeeId, newLeaderId);

            return Ok(result);
        }

        /// <summary>
        /// Удаляет сотрудника по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сотрудника, которого нужно удалить.</param>
        /// <returns>Результат операции удаления.</returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Employee? employee = _employeesStore.GetById(id);
            if (employee == null)
            {
                _logger.LogWarning($"Сотрудник id:{id} не найден");
                return NotFound($"Сотрудник id:{id} не найден");
            }

            bool result = await _employeesStore.DeleteAsync(id);

            return Ok(result);
        }

        /// <summary>
        /// Получить всех сотрудников.
        /// </summary>
        /// <returns>Список сотрудников.</returns>
        [HttpGet("getall")]
        public IActionResult GetAllEmployyes()
        {
            IEnumerable<EmployeeViewModel> employeesVM = _employeesStore.GetAll().AsEnumerable().ToViewModel();

            return Ok(employeesVM);
        }

        /// <summary>
        /// Получить всех неуволенных сотрудников.
        /// </summary>
        /// <returns>Список неуволенных сотрудников.</returns>
        [HttpGet("getallemployed")]
        public IActionResult GetAllEmployedEmployyes()
        {
            IEnumerable<EmployeeViewModel> employeesVM = _employeesStore.GetAllEmployed().AsEnumerable().ToViewModel();

            return Ok(employeesVM);
        }

        /// <summary>
        /// Получить сотрудника по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сотрудника.</param>
        /// <returns>Данные сотрудника.</returns>
        [HttpGet("employee/{id}")]
        public IActionResult Get(int id)
        {
            EmployeeViewModel? employeeVM = _employeesStore.GetById(id)?.ToViewModel();
            if(employeeVM == null)
            {
                _logger.LogWarning($"Сотрудник id:{id} не найден");
                return NotFound($"Сотрудник id:{id} не найден");
            }

            return Ok(employeeVM);
        }

        /// <summary>
        /// Установить тестовые данные.
        /// </summary>
        /// <returns>Список добавленных сотрудников.</returns>
        [HttpGet("settestdata")]
        public async Task<IActionResult> SetTestData(int count)
        {
            if(count < MIN_COUNT || count > MAX_COUNT)
            {
                _logger.LogWarning($"Число {count} должно быть в промежутке [{MIN_COUNT}:{MAX_COUNT}]");
                return BadRequest($"Число {count} должно быть в промежутке [{MIN_COUNT}:{MAX_COUNT}]");
            }

            bool deletedAllResult = await _employeesStore.DeleteAllAsync();
            if(!deletedAllResult)
            {
                _logger.LogWarning($"Ошибка при удалении всех сотрудников");
                return BadRequest($"Ошибка при удалении всех сотрудников");
            }

            List<AddedEmployeeViewModel> employeeViewModels = [];

            Employee testDirector = await _employeesStore.AddAsync(TestData.GetTestDirector());
            Employee testManager = await _employeesStore.AddAsync(TestData.GetTestManager(testDirector.Id));
            Employee testTeamLead = await _employeesStore.AddAsync(TestData.GetTestTeamLead(testDirector.Id));
            List<Employee> leaders = [testDirector, testManager, testTeamLead];

            employeeViewModels.AddRange(leaders.ToAddedViewModel());

            ICollection<AddedEmployeeViewModel> testEmployeesVM1 = TestData.GetTestEmployees(count/2, leaders.Skip(1).ToLeaderPosition().ToList());
            IEnumerable<Employee> testEmployees1 = await _employeesStore.AddRangeAsync(testEmployeesVM1);
            employeeViewModels.AddRange(testEmployees1.ToAddedViewModel());

            ICollection<AddedEmployeeViewModel> testEmployeesVM2 = TestData.GetTestEmployees(count / 2, testEmployees1.ToLeaderPosition().ToList());
            IEnumerable<Employee> testEmployees2 = await _employeesStore.AddRangeAsync(testEmployeesVM2);
            employeeViewModels.AddRange(testEmployees2.ToAddedViewModel());

            return Ok(employeeViewModels);
        }
    }
}
