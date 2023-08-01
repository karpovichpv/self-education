namespace DutchTreat.Services
{
    public interface IMailService
    {
        void SendMessage(string email, string subject, string message);
    }
}