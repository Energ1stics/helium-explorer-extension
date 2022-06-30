using HeliumApi.Models;
using HeliumApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Change Console
builder.Logging.AddSimpleConsole(options =>
{
    options.TimestampFormat = "[hh:mm:ss]";
});

// Add services to the container.
builder.Services.Configure<HeliumStatsDatabaseSettings>(
    builder.Configuration.GetSection("HeliumStatsDatabase"));

builder.Services.AddSingleton<DailyStatsService>();
builder.Services.AddSingleton<HeliumApiService>();
builder.Services.AddHostedService<DailyStatsUpdateHostedService>();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(builder =>
        builder.AllowAnyOrigin()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
