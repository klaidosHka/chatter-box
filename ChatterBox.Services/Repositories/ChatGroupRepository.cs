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

        public IQueryable<ChatGroup> Get()
        {
            return _dbContext.ChatGroups;
        }

        public void Import(ChatGroup group)
        {
            _dbContext.ChatGroups.Add(group);

            _dbContext.SaveChanges();
        }

        public void Import(IEnumerable<ChatGroup> groups)
        {
            _dbContext.ChatGroups.AddRange(groups);

            _dbContext.SaveChanges();
        }
    }
}
