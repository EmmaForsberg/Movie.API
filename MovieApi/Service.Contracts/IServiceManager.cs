namespace MovieApi.Service.Contracts
{
    public interface IServiceManager
    {
        IActorService ActorService { get; }
        IMovieService MovieService { get; }
        IReviewService ReviewService { get; }
    }
}
