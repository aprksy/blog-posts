using api.Domain.Interfaces;
using api.Domain.Entities;
using api.Application.Interfaces;

namespace api.Application.Usecases
{
    public class SearchClients : ISearchClients
    {
        private readonly IClientRepository _clientRepository;

        public SearchClients(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client[]> Handle(string name)
        {
            return await _clientRepository.Search(name);
        }
    }
}