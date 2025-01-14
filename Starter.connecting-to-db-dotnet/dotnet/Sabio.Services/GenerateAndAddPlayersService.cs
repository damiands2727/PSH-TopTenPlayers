using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sabio.Data.Providers;
using Sabio.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class GenerateAndAddPlayersService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    IDataProvider _data = null;
    public GenerateAndAddPlayersService(IServiceProvider serviceProvider, IDataProvider data)
    {
        _serviceProvider = serviceProvider;
        _data = data;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var playerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();

                try
                {
                    var request = await playerService.GeneratePlayersFromApi();
                    playerService.AddMany(request);
                    var procName = "[dbo].[StatsHistory_Insert]"; 

                    _data.ExecuteNonQuery(procName, paramCol =>
                    {
                        paramCol.AddWithValue("@GeneratedAt", DateTime.UtcNow);
                    });
                    Console.WriteLine("Batch of players inserted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
