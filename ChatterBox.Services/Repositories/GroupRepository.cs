using ChatterBox.Context;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;

namespace ChatterBox.Services.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly CbDbContext _dbContext;

        public GroupRepository(CbDbContext cbDbContext)
        {
            _dbContext = cbDbContext;
        }

        public IQueryable<Group> Get()
        {
            return _dbContext.Groups;
        }

        public void Import(Group group)
        {
            _dbContext.Groups.Add(group);
            _dbContext.SaveChanges();
        }

        public void Import(IEnumerable<Group> groups)
        {
            _dbContext.Groups.AddRange(groups);
            _dbContext.SaveChanges();
        }
    }
}
