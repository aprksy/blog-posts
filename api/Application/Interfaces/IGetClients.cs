using api.Domain.Entities;

namespace api.Application.Interfaces
{
    public interface IGetClients
    {
        Task<Client[]> Handle();
    }
}