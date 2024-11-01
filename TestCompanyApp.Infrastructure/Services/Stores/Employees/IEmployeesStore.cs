using TestCompanyApp.Domain.Entity;
using TestCompanyApp.Domain.ViewModels;

namespace TestCompanyApp.Infrastructure.Services.Stores.Employees
{
    /// <summary>
    /// Интерфейс для управления хранилищем сотрудников.
    /// Предоставляет методы для добавления, удаления, получения и обновления информации о сотрудниках.
    /// </summary>
    public interface IEmployeesStore
    {
        /// <summary>
        /// Добавляет нового сотрудника в хранилище.
        /// </summary>
        /// <param name="employeeVM">Модель представления нового сотрудника.</param>
        /// <returns>Добавленный сотрудник.</returns>
        public Task<Employee> AddAsync(AddedEmployeeViewModel employeeVM);

        /// <summary>
        /// Добавляет коллекцию моделей представлениясотрудников в хранилище.
        /// </summary>
        /// <param name="employeesVM">Коллекция сотрудников, которую нужно добавить.</param>
        /// <returns>Запрос добавленных сотрудников.</returns>
        public Task<IQueryable<Employee>> AddRangeAsync(IEnumerable<AddedEmployeeViewModel> employeesVM);

        /// <summary>
        /// Удаляет сотрудника из хранилища по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сотрудника, которого нужно удалить.</param>
        /// <returns>true, если удаление прошло успешно; иначе false.</returns>
        public Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Удаляет всех сотрудников из хранилища
        /// </summary>
        /// <returns></returns>
        public Task<bool> DeleteAllAsync();

        /// <summary>
        /// Уволяет сотрудника по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сотрудника, которого нужно уволить.</param>
        /// <param name="newLeaderId">Новый руководитель вместо даннаго сотрудника</param>
        /// <returns>true, если увольнение прошло успешно; иначе false.</returns>
        public Task<bool> FireAsync(int id, int? newLeaderId = null);

        /// <summary>
        /// Получает всех сотрудников из хранилища.
        /// </summary>
        /// <returns>Запрос для получения всех сотрудников.</returns>
        public IQueryable<Employee> GetAll();

        /// <summary>
        /// Получает всех действующих сотрудников из хранилища.
        /// </summary>
        /// <returns>Запрос для получения всех действующих сотрудников.</returns>
        public IQueryable<Employee> GetAllEmployed();

        /// <summary>
        /// Возвращает всех подчинённых для заданного руководителя.
        /// </summary>
        /// <param name="leaderId">Идентификатор руководителя.</param>
        /// <returns>Запрос для получения всех подчинённых сотрудников указанного руководителя.</returns>
        public IQueryable<Employee> GetAllSubordinates(int leaderId);

        /// <summary>
        /// Получает сотрудника по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сотрудника.</param>
        /// <returns>Сотрудник с заданным идентификатором или null, если не найден.</returns>
        public Employee? GetById(int id);

        /// <summary>
        /// Проверяет, есть ли у сотрудника подчиненные.
        /// </summary>
        /// <param name="id">Идентификатор сотрудника.</param>
        /// <returns>
        /// true, если у сотрудника есть подчиненные; 
        /// false, если подчиненных нет; 
        /// null, если сотрудник с указанным идентификатором не найден.
        /// </returns>
        public bool? HasSubordinates(int id);

        /// <summary>
        /// Проверяет, является ли сотрудник действующим по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сотрудника.</param>
        /// <returns>
        /// true, если сотрудник активен; 
        /// false, если сотрудник уволен; 
        /// null, если сотрудник с указанным идентификатором не найден.
        /// </returns>
        public bool? IsEmployed(int id);

        /// <summary>
        /// Устанавливает нового руководителя для подчиненного сотрудника.
        /// </summary>
        /// <param name="subordinateId">Идентификатор подчиненного.</param>
        /// <param name="leaderId">Идентификатор нового руководителя.</param>
        /// <returns></returns>
        public Task<bool> SetLeaderAsync(int subordinateId, int leaderId);

        /// <summary>
        /// Меняет руководителя для группы подчиненных.
        /// </summary>
        /// <param name="oldLeaderId">Идентификатор старого руководителя.</param>
        /// <param name="newLeaderId">Идентификатор нового руководителя.</param>
        /// <returns>true, если замена прошла успешно; иначе false.</returns>
        public Task<bool> SetNewLeaderAsync(int oldLeaderId, int newLeaderId);
    }
}
