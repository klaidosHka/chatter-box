using ChatterBox.Context;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;

namespace ChatterBox.Services.Repositories
{
    public class ChatGroupMessageRepository : IChatGroupMessageRepository
    {
        private readonly CbDbContext _dbContext;

        public ChatGroupMessageRepository(CbDbContext cbDbContext)
        {
            _dbContext = cbDbContext;
        }

        public IQueryable<ChatGroupMessage> Get()
        {
            return _dbContext.ChatGroupMessages;
        }

        public void Import(ChatGroupMessage groupMessage)
        {
            _dbContext.ChatGroupMessages.Add(groupMessage);

            _dbContext.SaveChanges();
        }

        public void Import(IEnumerable<ChatGroupMessage> groupMessages)
        {
            _dbContext.ChatGroupMessages.AddRange(groupMessages);

            _dbContext.SaveChanges();
        }
    }
}
