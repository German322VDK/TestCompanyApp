using TestCompanyApp.Domain.Entity;
using TestCompanyApp.Domain.ViewModels;

namespace TestCompanyApp.Infrastructure.StaticData
{
    /// <summary>
    /// Класс для генерации тестовых данных сотрудников.
    /// </summary>
    public static class TestData
    {
        private static readonly Random _random = new();

        private static readonly Dictionary<int, string> _firstNames = new()
        {
            { 1, "Иван" },
            { 2, "Петр" },
            { 3, "Сидор" },
            { 4, "Алексей" },
            { 5, "Дмитрий" },
            { 6, "Александр" },
            { 7, "Максим" },
            { 8, "Денис" },
            { 9, "Анатолий" },
            { 10, "Владимир" }
        };

        private static readonly Dictionary<int, string> _patronymics = new()
        {
            { 1, "Иванович" },
            { 2, "Петрович" },
            { 3, "Сидорович" },
            { 4, "Алексеевич" },
            { 5, "Дмитриевич" },
            { 6, "Александрович" },
            { 7, "Максимович" },
            { 8, "Денисович" },
            { 9, "Анатольевич" },
            { 10, "Владимирович" }
        };

        private static readonly Dictionary<int, string> _surNames = new()
        {
            { 1, "Иванов" },
            { 2, "Петров" },
            { 3, "Сидоров" },
            { 4, "Алексеев" },
            { 5, "Дмитриев" },
            { 6, "Александров" },
            { 7, "Максимов" },
            { 8, "Денисов" },
            { 9, "Анатольев" },
            { 10, "Владимиров" }
        };


        /// <summary>
        /// Генерирует тестового директора.
        /// </summary>
        /// <returns>Объект EmployeeViewModel, представляющий директора.</returns>
        public static AddedEmployeeViewModel GetTestDirector() =>
            new()
            {
                FirstName = _firstNames[1],
                Patronymic = _patronymics[1],
                SurName = _surNames[1],
                JobTitle = Position.Director
            };

        /// <summary>
        /// Генерирует тестового менеджера с указанным идентификатором руководителя.
        /// </summary>
        /// <param name="leaderId">Идентификатор руководителя.</param>
        /// <returns>Объект EmployeeViewModel, представляющий менеджера.</returns>
        public static AddedEmployeeViewModel GetTestManager(int leaderId) =>
           new()
           {
               FirstName = _firstNames[2],
               Patronymic = _patronymics[2],
               SurName = _surNames[2],
               JobTitle = Position.Manager,
               LeaderId = leaderId
           };

        /// <summary>
        /// Генерирует тестового тимлида с указанным идентификатором руководителя.
        /// </summary>
        /// <param name="leaderId">Идентификатор руководителя.</param>
        /// <returns>Объект EmployeeViewModel, представляющий тимлида.</returns>
        public static AddedEmployeeViewModel GetTestTeamLead(int leaderId) =>
           new()
           {
               FirstName = _firstNames[3],
               Patronymic = _patronymics[3],
               SurName = _surNames[3],
               JobTitle = Position.TeamLead,
               LeaderId = leaderId
           };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="leadersPos"></param>
        /// <returns></returns>
        public static ICollection<AddedEmployeeViewModel> GetTestEmployees(int count, List<LeaderPosition> leadersPos)
        {
            var employees = new List<AddedEmployeeViewModel>();

            for (int i = 0; i < count; i++)
            {
                LeaderPosition leaderPosition = GetRandomValue(leadersPos);

                employees.Add(new AddedEmployeeViewModel
                {
                    FirstName = GetRandomValue(_firstNames),
                    Patronymic = GetRandomValue(_patronymics),
                    SurName = GetRandomValue(_surNames),
                    JobTitle = PositionSwitcher(leaderPosition.JobTitle),
                    LeaderId = leaderPosition.LeaderId
                });
            }

            return employees;
        }

        private static string GetRandomValue(Dictionary<int, string> dictionary)
        {
            int randomIndex = _random.Next(1, dictionary.Count + 1);
            return dictionary[randomIndex];
        }

        private static Position PositionSwitcher(Position position)
        {
            var directorPositions = new[] { Position.Manager, Position.TeamLead };
            var managerPositions = new[] { Position.Administrator, Position.SupportSpecialist };
            var teamLeadPositions = new[] { Position.Developer, Position.Designer, Position.Analyst, Position.QAEngineer };
            var jobTitle = position switch
            {
                Position.Director => directorPositions[_random.Next(directorPositions.Length)],
                Position.Manager => managerPositions[_random.Next(managerPositions.Length)],
                Position.TeamLead => teamLeadPositions[_random.Next(teamLeadPositions.Length)],
                _ => Position.SupportSpecialist,
            };
            return jobTitle;
        }

        private static LeaderPosition GetRandomValue(List<LeaderPosition> list)
        {
            int randomIndex = _random.Next(list.Count);
            return list[randomIndex];
        }
    }
}
