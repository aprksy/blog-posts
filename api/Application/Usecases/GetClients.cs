using api.Domain.Interfaces;
using api.Domain.Entities;
using api.Application.Interfaces;

namespace api.Application.Usecases 
{
    public class GetClients : IGetClients
    {
        private readonly IClientRepository _clientRepository;

        public GetClients(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client[]> Handle()
        {
            return await _clientRepository.Get();
        }
    }
}