using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Bogus;
using Microsoft.EntityFrameworkCore;
using MovieCore.Entities;
using Microsoft.Extensions.DependencyInjection;
using MovieData;

namespace MovieData.Services
{
    //singelton service, denna körs bara en gång för att det är en singelton
    public class DataSeedHostingService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DataSeedHostingService> _logger;
        private static readonly Faker faker = new("sv");


        public DataSeedHostingService(IServiceProvider serviceProvider, ILogger<DataSeedHostingService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MovieContext>();

            // Kolla om det redan finns filmer
            if (await context.Movies.AnyAsync(cancellationToken))
            {
                _logger.LogInformation("Databasen är redan seedad.");
                return;
            }

            _logger.LogInformation("Startar seeding...");

            // Hämta genres
            var genres = context.Genres.ToList();

            if (!genres.Any())
            {
                genres = GenerateGenres();
                context.Genres.AddRange(genres);
                await context.SaveChangesAsync(cancellationToken);
            }

            // Skapa filmer
            var movies = GenerateMovies(10, genres);
            context.Movies.AddRange(movies);

            // Lägg till MovieDetails till varje film
            GenerateMovieDetails(movies);
            await context.SaveChangesAsync(cancellationToken);

            // Lägg till skådespelare
            var actors = GenerateActors(10, movies);
            context.Actors.AddRange(actors);
            await context.SaveChangesAsync(cancellationToken);

            // Lägg till MovieActor-relationer (film-skådespelare)
            var movieActors = GenerateMovieActors(movies, actors);
            context.Set<MovieActor>().AddRange(movieActors);
            await context.SaveChangesAsync(cancellationToken);

            // Lägg till recensioner
            var reviews = GenerateReviews(movies);
            context.Reviews.AddRange(reviews);
            await context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Seeding klar.");
        }

        // ----------------------
        // 👇 Seed-metod
        private static IEnumerable<Movie> GenerateMovies(int count, IEnumerable<Genre> genres)
        {
            var titles = new List<string>
        {
            "Inception",
            "Interstellar",
            "The Godfather",
            "Pulp Fiction",
            "Amélie",
            "The Matrix",
            "The Grand Budapest Hotel",
            "La La Land",
            "Forrest Gump",
            "Parasite"
        };

            var faker = new Faker("sv");
            var movies = new List<Movie>();

            for (int i = 0; i < count; i++)
            {
                var genre = faker.PickRandom(genres);

                movies.Add(new Movie
                {
                    Title = faker.PickRandom(titles),
                    Year = faker.Date.Past(40).Year,
                    Duration = faker.Random.Int(80, 180),
                    GenreId = genre.Id,
                    Genre = genre
                });
            }

            return movies;
        }

        private static IEnumerable<Actor> GenerateActors(int count, IEnumerable<Movie> movies)
        {
            var actorNames = new List<string>
            {
                 "Leonardo DiCaprio",
                 "Natalie Portman",
                 "Morgan Freeman",
                 "Scarlett Johansson",
                 "Tom Hanks",
                 "Emma Stone",
                 "Denzel Washington",
                 "Alicia Vikander",
                 "Mads Mikkelsen",
                 "Noomi Rapace"
            };

            var faker = new Bogus.Faker("sv");
            var actors = new List<Actor>();

            foreach (var name in actorNames)
            {
                var actor = new Actor
                {
                    Name = name,
                    BirthYear = faker.Random.Int(1960, 2000)
                };

                actors.Add(actor);
            }

            return actors;
        }
        private static List<Genre> GenerateGenres()
        {
            return new List<Genre>
            {
               new Genre { Name = "Action" },
               new Genre { Name = "Drama" },
               new Genre { Name = "Sci-Fi" },
               new Genre { Name = "Romance" },
               new Genre { Name = "Comedy" }
            };
        }

        private static void GenerateMovieDetails(IEnumerable<Movie> movies)
        {
            var languages = new[] { "English", "Swedish", "French", "Spanish", "German" };

            foreach (var movie in movies)
            {
                movie.MovieDetails = new MovieDetails
                {
                    Synopsis = faker.Lorem.Paragraph(),
                    Language = faker.PickRandom(languages),
                    Budget = faker.Random.Int(1_000_000, 100_000_000)
                };
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
                        ReviewerName = faker.Name.FullName(),
                        Comment = faker.Lorem.Sentence(10),
                        Rating = faker.Random.Int(1, 5),
                        MovieId = movie.Id
                    };

                    movie.Reviews.Add(review); // Lägg till i navigation property
                    reviews.Add(review);       // Lägg till i listan för save
                }
            }

            return reviews;
        }

        private static IEnumerable<MovieActor> GenerateMovieActors(IEnumerable<Movie> movies, IEnumerable<Actor> actors)
        {
            var rnd = new Random();
            var result = new List<MovieActor>();

            foreach (var actor in actors)
            {
                var selectedMovies = movies.OrderBy(_ => rnd.Next()).Take(faker.Random.Int(1, 3));

                foreach (var movie in selectedMovies)
                {
                    result.Add(new MovieActor
                    {
                        MovieId = movie.Id,
                        ActorId = actor.Id,
                        Role = faker.Name.JobTitle()
                    });
                }
            }

            return result;
        }
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;


    }

}

