using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Json2Kafka.Services
{
    public interface IUserService
    {
        bool IsValidUser(string userName, string password);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _Configuration;
        // inject database for user validation
        public UserService(ILogger<UserService> logger,IConfiguration Configuration)
        {
            _logger = logger;
            _Configuration = Configuration;
        }

        public bool IsValidUser(string userName, string password)
        {
            // Si dans la conf on a activé la sécurité sur l'API (aucun rapport avec l'authentification Kafka)
            if (bool.Parse( _Configuration["BasicAuthEnabled"] ))
            {
                _logger.LogInformation($"Validating user [{userName}]");
                if (string.IsNullOrWhiteSpace(userName)) return false;
                if (string.IsNullOrWhiteSpace(password)) return false;
                if (userName != _Configuration["BasicAuthLogin"] ) return false;
                if (password != _Configuration["BasicAuthPassword"] ) return false;
            }
            return true;
        }
    }
}
