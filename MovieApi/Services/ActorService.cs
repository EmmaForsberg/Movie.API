using MovieApi.Service.Contracts;
using MovieCore.DomainContracts;

namespace MovieApi.Services
{
    public class ActorService : IActorService
    {
       private IUnitOfWork uow;

        public ActorService(IUnitOfWork uow)
        {
            this.uow = uow;
        }
    }
}