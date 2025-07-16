using AutoMapper;
using MovieApi.Service.Contracts;
using MovieCore.DomainContracts;

namespace MovieApi.Services
{
    //denna hanterar bara inistansiering av våra servies
    public class ServiceManager : IServiceManager
    {
        //Skapa ServiceManager som returnerar instanser av varje service
       
            private readonly Lazy<IActorService> _actorService;
            private readonly Lazy<IMovieService> _movieService;
            private readonly Lazy<IReviewService> _reviewService;

            public IActorService ActorService => _actorService.Value;
            public IMovieService MovieService => _movieService.Value;
            public IReviewService ReviewService => _reviewService.Value;

            public ServiceManager(IUnitOfWork uow, IMapper mapper)
            {
                _actorService = new Lazy<IActorService>(() => new ActorService(uow, mapper));
                _movieService = new Lazy<IMovieService>(() => new MovieService(uow, mapper));
                _reviewService = new Lazy<IReviewService>(() => new ReviewService(uow, mapper));
            }
    }
}
