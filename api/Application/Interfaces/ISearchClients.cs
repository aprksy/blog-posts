using api.Domain.Entities;

namespace api.Application.Interfaces
{
    public interface ISearchClients
    {
        Task<Client[]> Handle(string name);
    }
}