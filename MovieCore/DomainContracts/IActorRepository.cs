using MovieCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCore.DomainContracts
{
    public interface IActorRepository
    {
        Task<Actor?> GetByNameAndBirthYearAsync(string name, int birthYear);
        void Add(Actor actor);
        Task<Actor?> GetActorWithMoviesAsync(int actorId);
        Task<IEnumerable<Actor>> GetAllAsync();
        Task<Actor?> GetAsync(int id);

    }
}
