using MovieApi.Service.Contracts;
using MovieCore.DomainContracts;

namespace MovieApi.Services
{
    //denna hanterar bara inistansiering av våra servies
    public class ServiceManager 
    {
        //Skapa ServiceManager som returnerar instanser av varje service

        private Lazy<IActorService> _actorService;
        private Lazy<IMovieService> _movieService;
        private Lazy<IReviewService> _reviewService;

        public IActorService Actor => _actorService.Value;
        public IMovieService Movie => _movieService.Value;
        public IReviewService Review => _reviewService.Value;

        public ServiceManager(IUnitOfWork uow) 
        {
            _actorService = new Lazy<IActorService>(() => new ActorService(uow));
            _movieService = new Lazy<IMovieService>(() => new MovieService(uow));
            _reviewService = new Lazy<IReviewService>(() => new ReviewService(uow));
        }

    }
}
