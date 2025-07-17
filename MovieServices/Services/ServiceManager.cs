using AutoMapper;
using MovieContracts;
using MovieServiceContracts.Service.Contracts;

namespace MovieServices.Services
{
    //denna hanterar bara inistansiering av våra servies
    public class ServiceManager : IServiceManager
    {
        //Skapa ServiceManager som returnerar instanser av varje service

        private readonly Lazy<IActorService> _actorService;
        private readonly Lazy<IMovieService> _movieService;
        private readonly Lazy<IReviewService> _reviewService;
        private readonly Lazy<IGenreService> _genreService;

        public IActorService ActorService => _actorService.Value;
        public IMovieService MovieService => _movieService.Value;
        public IReviewService ReviewService => _reviewService.Value;
        public IGenreService GenreService => _genreService.Value;

        public ServiceManager(IUnitOfWork uow, IMapper mapper)
        {
            _actorService = new Lazy<IActorService>(() => new ActorService(uow, mapper));
            _movieService = new Lazy<IMovieService>(() => new MovieService(uow, mapper));
            _reviewService = new Lazy<IReviewService>(() => new ReviewService(uow, mapper));
            _genreService = new Lazy<IGenreService>(() => new GenreService(uow,mapper));
        }
    }
}
