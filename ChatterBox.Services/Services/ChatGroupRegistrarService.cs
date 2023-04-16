using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using ChatterBox.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace ChatterBox.Services.Services
{
    public class ChatGroupRegistrarService : IChatGroupRegistrarService
    {
        private readonly IChatGroupRegistrarRepository _registrarRepository;

        public ChatGroupRegistrarService(IChatGroupRegistrarRepository registrarRepository)
        {
            _registrarRepository = registrarRepository;
        }

        public IEnumerable<ChatGroupRegistrar> Get()
        {
            return _registrarRepository.Get();
        }

        public IEnumerable<ChatGroupRegistrar> GetAsNoTracking()
        {
            return _registrarRepository
                .Get()
                .AsNoTracking();
        }

        public async Task ImportAsync(ChatGroupRegistrar registrar)
        {
            await _registrarRepository.ImportAsync(registrar);
        }

        public async Task ImportAsync(IEnumerable<ChatGroupRegistrar> registrars)
        {
            await _registrarRepository.ImportAsync(registrars);
        }
    }
}
