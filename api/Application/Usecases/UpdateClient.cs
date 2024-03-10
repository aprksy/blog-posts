using api.Domain.Interfaces;
using api.Domain.Entities;
using api.Application.Interfaces;
using api.Infrastructure.Services.EmailSender;
using api.Infrastructure.Services.DocSynchronizer;

namespace api.Application.Usecases
{
    public class UpdateClient : IUpdateClient
    {
        private readonly IClientRepository _clientRepository;
        private readonly IEmailSender _emailSender;
        private readonly IDocSynchronizer _docSynchronizer;

        public UpdateClient(IClientRepository clientRepository, IEmailSender emailSender, IDocSynchronizer docSynchronizer)
        {
            _clientRepository = clientRepository;
            _docSynchronizer = docSynchronizer;
            _emailSender = emailSender;
        }

        public async Task Handle(Client client)
        {
            await _clientRepository.Update(client);
            await _emailSender.Send(client.Email, "Hi there - welcome to my Carepatron portal.");
            await _docSynchronizer.SyncDocumentsFromExternalSource(client.Email);
        }

        public async Task<Client> GetById(string id)
        {
            return await _clientRepository.GetById(id);
        }
    }
}