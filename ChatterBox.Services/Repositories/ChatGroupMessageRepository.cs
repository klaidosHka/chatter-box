using ChatterBox.Context;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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
            return _dbContext.ChatGroupMessages.Include(m => m.Sender);
        }

        public async Task ImportAsync(ChatGroupMessage groupMessage)
        {
            await _dbContext.ChatGroupMessages.AddAsync(groupMessage);

            await _dbContext.SaveChangesAsync();
        }

        public async Task ImportAsync(IEnumerable<ChatGroupMessage> groupMessages)
        {
            await _dbContext.ChatGroupMessages.AddRangeAsync(groupMessages);

            await _dbContext.SaveChangesAsync();
        }
    }
}
