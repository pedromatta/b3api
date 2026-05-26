using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=dbquotes.db";

// Adiciona o Banco de Dados em Memória
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Adiciona o serviço HttpClient
builder.Services.AddHttpClient<B3IntegrationService>();
builder.Services.AddHostedService<DailySyncWorker>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();
