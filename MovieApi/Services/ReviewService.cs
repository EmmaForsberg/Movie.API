using MovieApi.Service.Contracts;
using MovieCore.DomainContracts;

namespace MovieApi.Services
{
    public class ReviewService : IReviewService
    {
        private IUnitOfWork uow;

        public ReviewService(IUnitOfWork uow)
        {
            this.uow = uow;
        }
    }
}
