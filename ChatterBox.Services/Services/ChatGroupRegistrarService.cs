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

        public void Import(ChatGroupRegistrar registrar)
        {
            _registrarRepository.Import(registrar);
        }

        public void Import(IEnumerable<ChatGroupRegistrar> registrars)
        {
            _registrarRepository.Import(registrars);
        }
    }
}
