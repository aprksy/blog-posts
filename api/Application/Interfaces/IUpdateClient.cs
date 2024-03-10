using api.Domain.Entities;

namespace api.Application.Interfaces
{
    public interface IUpdateClient
    {
        Task Handle(Client client);
        Task<Client> GetById(string id);
    }
}