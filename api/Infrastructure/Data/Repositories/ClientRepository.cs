using api.Domain.Entities;
using api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Infrastructure.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DataContext dataContext;

        public ClientRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task Create(Client client)
        {
            await dataContext.AddAsync(client);
            await dataContext.SaveChangesAsync();
        }

        public async Task<Client[]> Get()
        {
            return await dataContext.Clients.ToArrayAsync();
        }

        public async Task<Client> GetById(string id)
        {
            return await dataContext.Clients.FindAsync(id);
        }

        public async Task Update(Client client)
        {
            var existingClient = await dataContext.Clients.FirstOrDefaultAsync(x => x.Id == client.Id);

            if (existingClient == null)
                return;

            existingClient.FirstName = client.FirstName;
            existingClient.LastName = client.LastName;
            existingClient.Email = client.Email;
            existingClient.PhoneNumber = client.PhoneNumber;

            await dataContext.SaveChangesAsync();
        }

        public async Task<Client[]> Search(string name)
        {
            var _name = name.ToLower();
            return await dataContext.Clients.Where(client => client.FirstName.ToLower() == _name || client.LastName.ToLower() == _name).ToArrayAsync();
        }
    }
}

