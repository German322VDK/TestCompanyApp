namespace TestCompanyApp.Client.Infrastructure.Services
{
    public interface IUrlService
    {
        public string API { get; }

        public string GetAllEmployed { get; }
    }

    public class UrlService : IUrlService
    {
        private readonly IConfiguration _configuration;
        private const string EMPLOYEES = "employees";

        public UrlService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string API => 
            _configuration["ApplicationSettings:DefaultApiUrl"]!;

        public string GetAllEmployed =>
            $"{API}/{EMPLOYEES}/getallemployed";
    }
}
