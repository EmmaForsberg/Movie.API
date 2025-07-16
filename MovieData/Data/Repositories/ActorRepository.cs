using Microsoft.EntityFrameworkCore;
using MovieContracts;
using MovieCore.Entities;

namespace MovieData.Data.Repositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly MovieContext _context;

        public ActorRepository(MovieContext context)
        {
            _context = context;
        }

        public void Add(Actor actor)
        {
            _context.Actors.Add(actor);
        }

        public async Task<Actor?> GetActorWithMoviesAsync(int actorId)
        {
            return await _context.Actors
       .Include(a => a.MovieActors)
       .FirstOrDefaultAsync(a => a.Id == actorId);
        }

        public async Task<IEnumerable<Actor>> GetAllAsync()
        {
            return await _context.Actors.ToListAsync();
        }

        public async Task<Actor?> GetAsync(int id)
        {
            return await _context.Actors.FindAsync(id);
        }

        public async Task<Actor?> GetByNameAndBirthYearAsync(string name, int birthYear)
        {
            return await _context.Actors
                .FirstOrDefaultAsync(a => a.Name == name && a.BirthYear == birthYear);
        }

    }
}
