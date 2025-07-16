using MovieApi.Service.Contracts;
using MovieCore.DomainContracts;

namespace MovieApi.Services
{
    public class MovieService : IMovieService
    {
        private IUnitOfWork uow;

        public MovieService(IUnitOfWork uow)
        {
            this.uow = uow;
        }
    }
}
