using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TestCompanyApp.Database.Context;

namespace TestCompanyApp.Infrastructure.Initializers
{

    /// <summary>
    /// Интерфейс инициализации бд.
    /// </summary>
    public interface IDbInitializer
    {
        /// <summary>
        /// Инициализирует базу данных, применяя все необходимые миграции и заполняя её начальными данными.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию инициализации.</returns>
        public Task InitializeAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    public class TestCompanyAppDbInitializer : IDbInitializer
    {
        private readonly TestCompanyAppDbContext _dbContext;
        private readonly ILogger<TestCompanyAppDbInitializer> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        public TestCompanyAppDbInitializer(TestCompanyAppDbContext dbContext, ILogger<TestCompanyAppDbInitializer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task InitializeAsync()
        {
            Stopwatch timer = Stopwatch.StartNew();

            _logger.LogInformation("Инициализация базы данных...");

            DatabaseFacade dbFacade = _dbContext.Database;

            if (dbFacade.GetPendingMigrations().Any())
            {
                _logger.LogInformation("Выполнение миграций...");

                await dbFacade.MigrateAsync();

                _logger.LogInformation("Выполнение миграций выполнено успешно");
            }
            else
            {
                _logger.LogInformation($"База данных находится в актуальной версии ({timer.Elapsed.TotalSeconds:0.0###} c)");
            }

            _logger.LogInformation($"Инициализация БД выполнена успешно {timer.Elapsed.TotalSeconds}");
            timer.Stop();
        }
    }
}
