using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestCompanyApp.Domain.Entity
{
    /// <summary>
    /// Сотрудник организации
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Уникальный идентификатор сотрудника.
        /// </summary>
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Имя сотрудника.
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
        public Position JobTitle  { get; set; }

        /// <summary>
        /// Указывает, является ли сотрудник действующим.
        /// </summary>
        public bool IsEmployed { get; set; } = true;

        /// <summary>
        /// Идентификатор непосредственного руководителя сотрудника.
        /// Значение может быть null, если у сотрудника нет руководителя.
        /// </summary>
        public int? LeaderId { get; set; }

        /// <summary>
        /// Непосредственный руководитель сотрудника.
        /// </summary>
        public virtual Employee? Leader { get; set; }

        /// <summary>
        /// Список подчинённых сотрудников.
        /// </summary>
        public virtual ICollection<Employee> Subordinates { get; set; } = new List<Employee>();
    }

    /// <summary>
    /// Должность
    /// </summary>
    public enum Position
    {
        /// <summary>
        /// Директор
        /// </summary>
        Director = 0,

        /// <summary>
        /// Менеджер
        /// </summary>
        Manager = 1,

        /// <summary>
        /// Тимлид
        /// </summary>
        TeamLead = 2,

        /// <summary>
        /// Разработчик
        /// </summary>
        Developer = 3,

        /// <summary>
        /// Тестировщик
        /// </summary>
        QAEngineer = 4,   
        
        /// <summary>
        /// Дизайнер
        /// </summary>
        Designer = 5,    
        
        /// <summary>
        /// Аналитик
        /// </summary>
        Analyst = 6,      
        
        /// <summary>
        /// Администратор
        /// </summary>
        Administrator = 7,      
        
        /// <summary>
        /// Специалист по поддержке
        /// </summary>
        SupportSpecialist = 8     
    }

}
