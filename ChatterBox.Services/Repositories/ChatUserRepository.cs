using ChatterBox.Context;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;

namespace ChatterBox.Services.Repositories
{
    public class ChatUserRepository : IChatUserRepository
    {
        private readonly CbDbContext _dbContext;

        public ChatUserRepository(CbDbContext cbDbContext)
        {
            _dbContext = cbDbContext;
        }

        public IQueryable<ChatUser> Get()
        {
            return _dbContext.ChatUsers;
        }

        public void Import(ChatUser user)
        {
            _dbContext.ChatUsers.Add(user);

            _dbContext.SaveChanges();
        }

        public void Import(IEnumerable<ChatUser> users)
        {
            _dbContext.ChatUsers.AddRange(users);

            _dbContext.SaveChanges();
        }
    }
}
