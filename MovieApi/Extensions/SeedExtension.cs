using MovieApi.Data;
using System.Diagnostics;

namespace MovieApi.Extensions
{
    public static class SeedExtension
    {

        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {

            //Skapar ett scope för att tillfälligt använda DI
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<MovieContext>();

                try
                {
                    await SeedData.InitAsync(context);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
            }
        }
    }
}
