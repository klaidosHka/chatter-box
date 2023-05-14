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

        public async Task RemoveAsync(ChatGroupRegistrar group)
        {
            var existingGroup = await _dbContext.ChatGroupsRegistrar.FindAsync(group.Id);
            
            if (existingGroup is not null)
            {
                _dbContext.ChatGroupsRegistrar.Remove(existingGroup);
                
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
