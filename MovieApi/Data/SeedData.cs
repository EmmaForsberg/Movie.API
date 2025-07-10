using Bogus;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models.Entities;
using System.Threading;
using static Bogus.DataSets.Name;

namespace MovieApi.Extensions
{
    public class SeedData
    {
        private static Faker faker = new Faker("sv");

        internal static async Task InitAsync(MovieContext context)
        {
            // Om det redan finns filmer, gör inget
            if (await context.Movies.AnyAsync()) return;

            var genres = GenerateGenres();
            await context.AddRangeAsync(genres);
            await context.SaveChangesAsync();

            var movies = GenerateMovies(20, genres);
            await context.AddRangeAsync(movies);
            await context.SaveChangesAsync();

            GenerateMovieDetails(movies);
            await context.SaveChangesAsync();

            var actors = GenerateActors(10,movies);
            await context.Actors.AddRangeAsync(actors);
            await context.SaveChangesAsync();

            var movieActors = new List<MovieActor>();
            var rnd = new Random();

            foreach (var actor in actors)
            {
                var numberOfMovies = rnd.Next(1, 4);
                var selectedMovies = movies.OrderBy(x => rnd.Next()).Take(numberOfMovies);

                foreach (var movie in selectedMovies)
                {
                    movieActors.Add(new MovieActor
                    {
                        MovieId = movie.Id,
                        ActorId = actor.Id,
                        Role = "Seeded Role"
                    });
                }
            }

            var moviesWithoutActors = movies
                .Where(m => !movieActors.Any(ma => ma.MovieId == m.Id))
                .ToList();

            foreach (var movie in moviesWithoutActors)
            {
                var randomActor = actors[rnd.Next(actors.Count)];
                movieActors.Add(new MovieActor
                {
                    MovieId = movie.Id,
                    ActorId = randomActor.Id,
                    Role = "Extra Assigned Role"
                });
            }

            await context.Set<MovieActor>().AddRangeAsync(movieActors);
            await context.SaveChangesAsync();

            var reviews = GenerateReviews(movies);
            await context.AddRangeAsync(reviews);
            await context.SaveChangesAsync();
        }



        private static IEnumerable<Movie> GenerateMovies(int count, IEnumerable<Genre> genres)
        {
            var movies = new List<Movie>();
           var random = new Random();

            for (int i = 0; i < count; i++)
            {
                var titel = faker.Commerce.Color() + faker.Lorem.Word();
                var year = faker.Random.Int(1960, 2024);
                var duration = faker.Random.Int(60, 200);
                var genre = faker.PickRandom(genres);

                var movie = new Movie
                {
                    Title = titel,
                    Year = year,
                    Duration = duration,
                    Genre = genre,
                    GenreId = genre.Id,
                };

                movies.Add(movie);
            }

            return movies;
        }

        private static List<Actor> GenerateActors(int count, IEnumerable<Movie> movies)
        {
            var actors = new List<Actor>();

            foreach (var i in Enumerable.Range(0, count))
            {
                var name = faker.Name.FullName();
                var birthyear = faker.Random.Int(1960, 2005);

                var actor = new Actor
                {
                    Name = name,
                    BirthYear = birthyear
                };

                actors.Add(actor);
            }

            return actors;
        }

        private static IEnumerable<Genre> GenerateGenres()
        {
          var genreNames = new List<string> { "Action", "Drama", "Comedy", "Thriller", "Sci-Fi" };

            var genres = genreNames.Select(name => new Genre { Name = name }).ToList();

            return genres;
        }


        private static void GenerateMovieDetails(IEnumerable<Movie> movies)
        {
            var languages = new List<string> { "English", "Swedish", "French", "Spanish", "German" };


            foreach (var movie in movies)
            {
                var synopsis = faker.Lorem.Paragraph();
                var language = faker.PickRandom(languages);
                var budget = faker.Random.Int(1_000_000, 100_000_000);

                var moviedetails = new MovieDetails
                {
                    Synopsis = synopsis,
                    Language = language,
                    Budget = budget
                };
                movie.MovieDetails = moviedetails;
            }
        }

        private static IEnumerable<Review> GenerateReviews(IEnumerable<Movie> movies)
        {
            var reviews = new List<Review>();

            foreach (var movie in movies)
            {
                var numberOfReviews = faker.Random.Int(0, 5);

                for (int i = 0; i < numberOfReviews; i++)
                {
                    var review = new Review
                    {
                        ReviewerName = faker.Name.FirstName(),
                        Comment = faker.Lorem.Text(),
                        Rating = faker.Random.Int(1, 5),
                        Movie = movie
                    };

                    movie.Reviews.Add(review);
                    reviews.Add(review);
                }
            }

            return reviews;
        }
    }
}
