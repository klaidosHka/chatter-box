using ChatterBox.Context;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;

namespace ChatterBox.Services.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly CbDbContext _dbContext;

        public MessageRepository(CbDbContext cbDbContext)
        {
            _dbContext = cbDbContext;
        }

        public IQueryable<Message> Get()
        {
            return _dbContext.Messages;
        }

        public void Import(Message message)
        {
            _dbContext.Messages.Add(message);
            _dbContext.SaveChanges();
        }

        public void Import(IEnumerable<Message> messages)
        {
            _dbContext.Messages.AddRange(messages);
            _dbContext.SaveChanges();
        }
    }
}
