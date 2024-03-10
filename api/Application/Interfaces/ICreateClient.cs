using api.Domain.Entities;

namespace api.Application.Interfaces
{
    public interface ICreateClient
    {
        Task Handle(Client client);
    }
}