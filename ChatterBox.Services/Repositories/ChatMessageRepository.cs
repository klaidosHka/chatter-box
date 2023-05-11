using ChatterBox.Context;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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
            return _dbContext.ChatMessages
                .Include(m => m.Sender)
                .Include(m => m.Receiver);
        }

        public async Task ImportAsync(ChatMessage message)
        {
            await _dbContext.ChatMessages.AddAsync(message);

            await _dbContext.SaveChangesAsync();
        }

        public async Task ImportAsync(IEnumerable<ChatMessage> messages)
        {
            await _dbContext.ChatMessages.AddRangeAsync(messages);

            await _dbContext.SaveChangesAsync();
        }
    }
}
