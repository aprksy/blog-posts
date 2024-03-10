using api.Domain.Entities;

namespace api.Infrastructure.Data
{
    public class DataSeeder
    {
        private readonly DataContext dataContext;

        public DataSeeder(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Seed()
        {
            var client = new Client("xosiosiosdhad", "John", "Smith", "john@gmail.com", "+18202820232");
            dataContext.Add(client);
            client = new Client("xosiosiosdhae", "Newton", "John", "newton@gmail.com", "+18202820233");
            dataContext.Add(client);

            dataContext.SaveChanges();
        }
    }
}

