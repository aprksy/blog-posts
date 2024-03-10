using api.Domain.Entities;

namespace api.Domain.Interfaces
{
    public interface IClientRepository
    {
        Task<Client[]> Get();
        Task<Client> GetById(string id);
        Task Create(Client client);
        Task Update(Client client);
        Task<Client[]> Search(string name);
    }
}

