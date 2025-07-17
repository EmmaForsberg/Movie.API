﻿namespace MovieServiceContracts.Service.Contracts
{
    public interface IServiceManager
    {
        IActorService ActorService { get; }
        IMovieService MovieService { get; }
        IReviewService ReviewService { get; }
        IGenreService GenreService { get; }
    }
}
