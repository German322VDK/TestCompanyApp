using System.ComponentModel.DataAnnotations;
using TestCompanyApp.Domain.Entity;

namespace TestCompanyApp.Domain.ViewModels
{
    /// <summary>
    /// Базовая модель представления для сотрудника
    /// </summary>
    public abstract class EmployeeBaseViewModel
    {
        /// <summary>
        /// Имя сотрудника
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество сотрудника.
        /// </summary>
        [Required]
        public string Patronymic { get; set; }

        /// <summary>
        /// Фамилия сотрудника.
        /// </summary>
        [Required]
        public string SurName { get; set; }

        /// <summary>
        /// Должность сотрудника в организации. 
        /// </summary>
        [Required]
        public Position JobTitle { get; set; }

        /// <summary>
        /// Идентификатор непосредственного руководителя сотрудника.
        /// Значение может быть null, если у сотрудника нет руководителя.
        /// </summary>
        public int? LeaderId { get; set; }
    }
   
}
