using MovieCore.DomainContracts;

namespace MovieData.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieContext _context;

        public IMovieRepository MovieRepository { get; }
        public IActorRepository ActorRepository { get; }
        public IReviewRepository ReviewRepository { get; }

        public UnitOfWork(
            MovieContext context,
            IMovieRepository movieRepository,
            IActorRepository actorRepository,
            IReviewRepository reviewRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            MovieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
            ActorRepository = actorRepository ?? throw new ArgumentNullException(nameof(actorRepository));
            ReviewRepository = reviewRepository ?? throw new ArgumentNullException(nameof(reviewRepository));
        }

        public async Task CompleteAsync() => await _context.SaveChangesAsync();
    }
}


