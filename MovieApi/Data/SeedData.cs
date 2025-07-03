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
            //kollar om det finns filmer
            if (await context.Movies.AnyAsync()) return;

            var genres = GenerateGenres();
            await context.AddRangeAsync(genres);
            await context.SaveChangesAsync();

            var movies = GenerateMovies(20, genres);
            await context.AddRangeAsync(movies);

            GenerateMovieDetails(movies);

            var actors = GenerateActors(10, movies);
            await context.AddRangeAsync(actors);

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

        private static IEnumerable<Actor> GenerateActors(int count, IEnumerable<Movie> movies)
        {
            var rnd = new Random();
            var actors = new List<Actor>();

            for (int i = 0; i < count; i++)
            {
            var numberOfMovies = rnd.Next(1, 4);
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

        private static IEnumerable<Genre> GenerateGenres()
        {
          var genreNames = new List<string> { "Action", "Drama", "Comedy", "Thriller", "Sci-Fi" };

            var genres = genreNames.Select(name => new Genre { Name = name }).ToList();

            return genres;
        }

        //denna metod behöver ej vara ienumerable för att den har 1-1 relation med movie
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
