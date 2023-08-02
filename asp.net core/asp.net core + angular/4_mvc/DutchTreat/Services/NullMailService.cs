using Microsoft.Extensions.Logging;

namespace DutchTreat.Services
{
    public class NullMailService : IMailService
    {
        private readonly ILogger<NullMailService> _logger;

        public NullMailService(ILogger<NullMailService> logger)
        {
            _logger = logger;
        }

        public void SendMessage(string email, string subject, string message)
        {
            _logger.LogInformation($"To {email}; subject: {subject}; body: {message}");
        }
    }
}
