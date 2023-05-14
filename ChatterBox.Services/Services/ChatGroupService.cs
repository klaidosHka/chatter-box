using System.Text.RegularExpressions;
using ChatterBox.Interfaces.Dto;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using ChatterBox.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace ChatterBox.Services.Services
{
    public class ChatGroupService : IChatGroupService
    {
        private const int MAX_OWNED_GROUPS = 3;

        private readonly IChatGroupRegistrarService _registrarService;
        private readonly IChatGroupRepository _repository;

        public ChatGroupService(IChatGroupRegistrarService registrarService, IChatGroupRepository repository)
        {
            _registrarService = registrarService;
            _repository = repository;
        }

        public async Task<CreateGroupResponse> CreateAsync(CreateGroupRequest request)
        {
            if (request.GroupName.Length > 10)
            {
                return new CreateGroupResponse
                {
                    MessageError = $"Name `{request.GroupName}` is too long. Only 10 characters are allowed.",
                    Success = false
                };
            }

            var groups = GetAsNoTracking().ToList();

            var groupsOwned = groups.Count(g => g.OwnerId == request.UserId);

            if (groupsOwned == MAX_OWNED_GROUPS)
            {
                return new CreateGroupResponse
                {
                    MessageError = $"Limit of {MAX_OWNED_GROUPS} owned groups is reached.",
                    Success = false
                };
            }

            var groupExists = groups.Any(g => g.Name.Equals(request.GroupName, StringComparison.OrdinalIgnoreCase));

            if (groupExists)
            {
                return new CreateGroupResponse
                {
                    MessageError = "Group of such name already exists.",
                    Success = false
                };
            }

            var group = new ChatGroup
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.GroupName,
                OwnerId = request.UserId
            };

            await ImportAsync(group);

            await _registrarService.ImportAsync(new ChatGroupRegistrar
            {
                GroupId = group.Id,
                Id = Guid.NewGuid().ToString(),
                UserId = group.OwnerId
            });

            return new CreateGroupResponse
            {
                GroupId = group.Id,
                MessageError = string.Empty,
                Success = true
            };
        }

        public IEnumerable<ChatGroup> Get()
        {
            return _repository.Get();
        }

        public IEnumerable<ChatGroup> GetAsNoTracking()
        {
            return _repository
                .Get()
                .AsNoTracking();
        }

        public IEnumerable<GroupMapped> GetMapped(string userId)
        {
            var groupsJoined = _registrarService
                .GetAsNoTracking()
                .Where(r => r.UserId == userId)
                .Select(r => r.GroupId)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return GetAsNoTracking()
                .Select(g => new GroupMapped
                {
                    Id = g.Id,
                    Joined = groupsJoined.Contains(g.Id),
                    Name = g.Name,
                    OwnerId = g.OwnerId
                })
                .ToList();
        }

        public IEnumerable<ChatGroup> GetUserGroups(string userId)
        {
            return _registrarService
                .GetAsNoTracking()
                .Select(r => new
                {
                    r.UserId,
                    r.GroupId
                })
                .Where(r => r.UserId == userId)
                .Join(
                    GetAsNoTracking(),
                    r => r.GroupId,
                    g => g.Id,
                    (r, g) => g
                )
                .ToList();
        }

        public async Task ImportAsync(ChatGroup group)
        {
            await _repository.ImportAsync(group);
        }

        public async Task ImportAsync(IEnumerable<ChatGroup> groups)
        {
            await _repository.ImportAsync(groups);
        }
    }
}