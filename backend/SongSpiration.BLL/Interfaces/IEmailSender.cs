using System.Threading.Tasks;

namespace SongSpiration.BLL.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(string toEmail, string subject, string message);
}