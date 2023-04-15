using ChatterBox.Context;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;

namespace ChatterBox.Services.Repositories
{
    public class ChatGroupRegistrarRepository : IChatGroupRegistrarRepository
    {
        private readonly CbDbContext _dbContext;

        public ChatGroupRegistrarRepository(CbDbContext cbDbContext)
        {
            _dbContext = cbDbContext;
        }

        public IQueryable<ChatGroupRegistrar> Get()
        {
            return _dbContext.ChatGroupsRegistrar;
        }

        public void Import(ChatGroupRegistrar registrar)
        {
            _dbContext.ChatGroupsRegistrar.Add(registrar);

            _dbContext.SaveChanges();
        }

        public void Import(IEnumerable<ChatGroupRegistrar> registrars)
        {
            _dbContext.ChatGroupsRegistrar.AddRange(registrars);

            _dbContext.SaveChanges();
        }
    }
}
