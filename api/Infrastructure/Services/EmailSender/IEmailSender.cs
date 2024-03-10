using api.Utilities;
using Microsoft.EntityFrameworkCore;

namespace api.Infrastructure.Services.EmailSender
{
    public interface IEmailSender
    {
        Task Send(string email, string message);
    }
}

