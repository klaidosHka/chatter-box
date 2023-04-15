using ChatterBox.Context;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;

namespace ChatterBox.Services.Repositories
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly CbDbContext _dbContext;

        public ChatMessageRepository(CbDbContext cbDbContext)
        {
            _dbContext = cbDbContext;
        }

        public IQueryable<ChatMessage> Get()
        {
            return _dbContext.ChatMessages;
        }

        public void Import(ChatMessage message)
        {
            _dbContext.ChatMessages.Add(message);

            _dbContext.SaveChanges();
        }

        public void Import(IEnumerable<ChatMessage> messages)
        {
            _dbContext.ChatMessages.AddRange(messages);

            _dbContext.SaveChanges();
        }
    }
}
