using ChatterBox.Context;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;

namespace ChatterBox.Services.Repositories
{
    public class ChatGroupRepository : IChatGroupRepository
    {
        private readonly CbDbContext _dbContext;

        public ChatGroupRepository(CbDbContext cbDbContext)
        {
            _dbContext = cbDbContext;
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<ChatGroup> Get()
        {
            return _dbContext.ChatGroups;
        }

        public async Task ImportAsync(ChatGroup group)
        {
            await _dbContext.ChatGroups.AddAsync(group);

            await _dbContext.SaveChangesAsync();
        }

        public async Task ImportAsync(IEnumerable<ChatGroup> groups)
        {
            await _dbContext.ChatGroups.AddRangeAsync(groups);

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(ChatGroup group)
        {
            var existingGroup = await _dbContext.ChatGroups.FindAsync(group.Id);

            if (existingGroup is not null)
            {
                _dbContext.ChatGroups.Remove(existingGroup);

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}