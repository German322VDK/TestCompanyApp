using TestCompanyApp.Domain.Entity;
using TestCompanyApp.Domain.ViewModels;

namespace TestCompanyApp.Infrastructure.Services.Mapping
{
    /// <summary>
    /// Класс, содержащий методы для преобразования объектов EmployeeViewModel в сущности Employee.
    /// </summary>
    public static class EmployeeMapper
    {
        #region EmployeeViewModel

        /// <summary>
        ///  Преобразует модель представления сотрудника в сущность Employee.
        /// </summary>
        /// <param name="employee">Модель представления сотрудника.</param>
        /// <returns>Сущность сотрудника или null, если передана null-модель.</returns>
        public static Employee? ToEntity(this EmployeeViewModel employee) => employee == null ? null :
            new Employee
            {
                FirstName = employee.FirstName,
                Patronymic = employee.Patronymic,
                SurName = employee.SurName,
                JobTitle = employee.JobTitle,
                LeaderId = employee.LeaderId,
                IsEmployed = true,
            };

        /// <summary>
        /// Преобразует коллекцию EmployeeViewModel в коллекцию Employee.
        /// </summary>
        /// <param name="employees">Коллекция моделей представления сотрудников.</param>
        /// <returns>>Коллекция сущностей сотрудников.</returns>
        public static IEnumerable<Employee> ToEntity(this IEnumerable<EmployeeViewModel> employees) =>
          employees?.Select(emp => emp.ToEntity())
            .Where(emp => emp != null)
            .Select(emp => emp!)
            ?? [];

        /// <summary>
        /// Преобразует экземпляр <see cref="Employee"/> в модель представления <see cref="EmployeeViewModel"/>.
        /// </summary>
        /// <param name="employee">Экземпляр сотрудника для преобразования.</param>
        /// <returns>
        /// Модель представления <see cref="EmployeeViewModel"/>, или <c>null</c>, если сотрудник не задан.
        /// </returns>
        public static EmployeeViewModel? ToViewModel(this Employee employee) => employee == null ? null :
           new EmployeeViewModel
           {
               FirstName = employee.FirstName,
               Patronymic = employee.Patronymic,
               SurName = employee.SurName,
               JobTitle = employee.JobTitle,
               LeaderId = employee.LeaderId,
               Id = employee.Id
           };


        /// <summary>
        /// Преобразует коллекцию <see cref="Employee"/> в коллекцию моделей представления <see cref="EmployeeViewModel"/>.
        /// </summary>
        /// <param name="employees">Коллекция сотрудников для преобразования.</param>
        /// <returns>
        /// Коллекция моделей представления <see cref="EmployeeViewModel"/>. Если коллекция сотрудников пуста, возвращает пустую коллекцию.
        /// </returns>
        public static IEnumerable<EmployeeViewModel> ToViewModel(this IEnumerable<Employee> employees) =>
          employees?.Select(emp => emp.ToViewModel())
            .Where(emp => emp != null)
            .Select(emp => emp!)
            ?? [];

        #endregion

        #region AddedEmployeeViewModel

        /// <summary>
        ///  Преобразует модель представления сотрудника в сущность Employee.
        /// </summary>
        /// <param name="employee">Модель представления сотрудника.</param>
        /// <returns>Сущность сотрудника или null, если передана null-модель.</returns>
        public static Employee? ToEntity(this AddedEmployeeViewModel employee) => employee == null ? null :
            new Employee
            {
                FirstName = employee.FirstName,
                Patronymic = employee.Patronymic,
                SurName = employee.SurName,
                JobTitle = employee.JobTitle,
                LeaderId = employee.LeaderId,
                IsEmployed = true,
            };

        /// <summary>
        /// Преобразует коллекцию AddedEmployeeViewModel в коллекцию Employee.
        /// </summary>
        /// <param name="employees">Коллекция моделей представления сотрудников.</param>
        /// <returns>>Коллекция сущностей сотрудников.</returns>
        public static IEnumerable<Employee> ToEntity(this IEnumerable<AddedEmployeeViewModel> employees) =>
          employees?.Select(emp => emp.ToEntity())
            .Where(emp => emp != null)
            .Select(emp => emp!)
            ?? [];

        /// <summary>
        /// Преобразует экземпляр <see cref="Employee"/> в модель представления <see cref="EmployeeViewModel"/>.
        /// </summary>
        /// <param name="employee">Экземпляр сотрудника для преобразования.</param>
        /// <returns>
        /// Модель представления <see cref="EmployeeViewModel"/>, или <c>null</c>, если сотрудник не задан.
        /// </returns>
        public static AddedEmployeeViewModel? ToAddedViewModel(this Employee employee) => employee == null ? null :
           new AddedEmployeeViewModel
           {
               FirstName = employee.FirstName,
               Patronymic = employee.Patronymic,
               SurName = employee.SurName,
               JobTitle = employee.JobTitle,
               LeaderId = employee.LeaderId
           };


        /// <summary>
        /// Преобразует коллекцию <see cref="Employee"/> в коллекцию моделей представления <see cref="EmployeeViewModel"/>.
        /// </summary>
        /// <param name="employees">Коллекция сотрудников для преобразования.</param>
        /// <returns>
        /// Коллекция моделей представления <see cref="EmployeeViewModel"/>. Если коллекция сотрудников пуста, возвращает пустую коллекцию.
        /// </returns>
        public static IEnumerable<AddedEmployeeViewModel> ToAddedViewModel(this IEnumerable<Employee> employees) =>
          employees?.Select(emp => emp.ToAddedViewModel())
            .Where(emp => emp != null)
            .Select(emp => emp!)
            ?? [];

        #endregion

        #region LeaderPosition

        /// <summary>
        /// Преобразует объект <see cref="Employee"/> в <see cref="LeaderPosition"/>.
        /// </summary>
        /// <param name="employee">Сотрудник, который будет преобразован.</param>
        /// <returns>Объект <see cref="LeaderPosition"/>, представляющий позицию лидера.</returns>
        public static LeaderPosition ToLeaderPosition(this Employee employee) =>
            new(employee.Id, employee.JobTitle);

        /// <summary>
        /// Преобразует коллекцию объектов <see cref="Employee"/> в коллекцию <see cref="LeaderPosition"/>.
        /// </summary>
        /// <param name="employees">Коллекция сотрудников, которые будут преобразованы.</param>
        /// <returns>Коллекция объектов <see cref="LeaderPosition"/>, представляющих позиции лидеров.</returns>
        public static IEnumerable<LeaderPosition> ToLeaderPosition(this IEnumerable<Employee> employees) =>
            employees.Select(emp => emp.ToLeaderPosition());

        #endregion


    }
}
