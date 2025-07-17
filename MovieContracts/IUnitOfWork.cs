using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieContracts
{
    public interface IUnitOfWork
    {
        IMovieRepository MovieRepository { get; }
        IReviewRepository ReviewRepository { get; }
        IActorRepository ActorRepository { get; }
        IGenreRepository GenreRepository { get; }

        Task CompleteAsync();
    }

}
