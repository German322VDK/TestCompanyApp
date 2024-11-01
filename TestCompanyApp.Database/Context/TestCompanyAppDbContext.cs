using Microsoft.EntityFrameworkCore;
using TestCompanyApp.Domain.Entity;

namespace TestCompanyApp.Database.Context
{
    /// <summary>
    /// Контекст базы данных для приложения TestCompanyApp.
    /// </summary>
    public class TestCompanyAppDbContext : DbContext
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TestCompanyAppDbContext"/> 
        /// с заданными параметрами конфигурации базы данных.
        /// </summary>
        /// <param name="options">Параметры конфигурации контекста базы данных.</param>
        public TestCompanyAppDbContext(DbContextOptions<TestCompanyAppDbContext> options) : base(options) { }

        /// <summary>
        /// Получает или устанавливает коллекцию сотрудников.
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Настраивает модель данных при помощи Fluent API.
        /// Здесь определяются связи между сущностями и другие конфигурации.
        /// </summary>
        /// <param name="modelBuilder">Объект для построения модели данных.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasOne(emp => emp.Leader)
                .WithMany(emp => emp.Subordinates)
                .HasForeignKey(emp => emp.LeaderId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
