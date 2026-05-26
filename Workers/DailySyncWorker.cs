public class DailySyncWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DailySyncWorker> _logger;

    public DailySyncWorker(IServiceProvider serviceProvider, ILogger<DailySyncWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Iniciando sincronização de cotações da B3...");

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var b3Service = scope.ServiceProvider.GetRequiredService<B3IntegrationService>();

                var quotes = await b3Service.FetchDailyQuotesAsync();

                dbContext.Quotes.RemoveRange(dbContext.Quotes);
                await dbContext.Quotes.AddRangeAsync(quotes, stoppingToken);
                await dbContext.SaveChangesAsync(stoppingToken);

                _logger.LogInformation("Cotações atualizadas com sucesso. Próxima atualização em 24h.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao sincronizar cotações.");
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}
