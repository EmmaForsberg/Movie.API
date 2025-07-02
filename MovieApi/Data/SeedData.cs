using Bogus;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models;
using System.Threading;

namespace MovieApi.Extensions
{
    public class SeedData
    {
        private static Faker faker = new Faker("sv");

        internal static async Task InitAsync(MovieContext context)
        {
            //kollar om det finns filmer
            if (await context.Movies.AnyAsync()) return;

            var movies = GenerateMovies(20);
            await context.AddRangeAsync(movies);


            var actors = GenerateActors(10, movies);
            await context.AddRangeAsync(actors);

        }

        private static IEnumerable<Movie> GenerateMovies(int count)
        {
            var movies = new List<Movie>();
            var genres = new List<string> { "Action", "Drama", "Comedy", "Thriller", "Sci-Fi" };

            for (int i = 0; i < count; i++)
            {
                // Slumpa titel
                // Slumpa genre från genres
                // Slumpa release year (t.ex. 1950-2024)
                // Slumpa description
                // Slumpa duration (80-180 min)

                // Skapa nytt Movie-objekt med dessa värden

                // Lägg till Movie i movies-listan
            }

            return movies;
        }

        private static IEnumerable<Actor> GenerateActors(int count, IEnumerable<Movie> movies)
        {
            var rnd = new Random();
            var actors = new List<Actor>();
            var numberOfMovies = rnd.Next(1, 4);

            for (int i = 0; i < count; i++)
            {
                var name = faker.Name.FullName();
                var birthyear = faker.Random.Int(1960, 2005);

                var actor = new Actor
                {
                    Name = name,
                    BirthYear = birthyear
                };

                var selectedMovies = faker.PickRandom(movies, numberOfMovies).ToList();

                actor.Movies = selectedMovies;
                actors.Add(actor);
            }
                return actors;
        }
    }
}
