using ChatterBox.Context;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using System.Runtime.CompilerServices;

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

        public async Task ImportAsync(ChatGroupRegistrar registrar)
        {
            await _dbContext.ChatGroupsRegistrar.AddAsync(registrar);

            await _dbContext.SaveChangesAsync();
        }

        public async Task ImportAsync(IEnumerable<ChatGroupRegistrar> registrars)
        {
            await _dbContext.ChatGroupsRegistrar.AddRangeAsync(registrars);

            await _dbContext.SaveChangesAsync();
        }
    }
}
