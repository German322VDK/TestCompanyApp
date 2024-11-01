using TestCompanyApp.Domain.Entity;

namespace TestCompanyApp.Domain.ViewModels
{
    /// <summary>
    /// Представляет позицию лидера в организации.
    /// </summary>
    public class LeaderPosition
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LeaderPosition"/>.
        /// </summary>
        /// <param name="leaderId">Уникальный идентификатор лидера.</param>
        /// <param name="jobTitle">Должность или наименование работы лидера.</param>
        public LeaderPosition(int leaderId, Position jobTitle)
        {
            LeaderId = leaderId;
            JobTitle = jobTitle;
        }

        /// <summary>
        /// Получает уникальный идентификатор лидера.
        /// </summary>
        public int LeaderId { get; init; }

        /// <summary>
        /// Получает должность лидера.
        /// </summary>
        public Position JobTitle { get; init; }
    }
}
