using HeliumApi.Models;
using HeliumApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Change Console
builder.Logging.AddSimpleConsole(options =>
{
    options.TimestampFormat = "[hh:mm:ss]";
});

builder.Services.AddCors();

// Add services to the container.
builder.Services.Configure<HeliumStatsDatabaseSettings>(
    builder.Configuration.GetSection("HeliumStatsDatabase"));

builder.Services.AddSingleton<DailyStatsService>();
builder.Services.AddSingleton<HeliumApiService>();
builder.Services.AddHostedService<DailyStatsUpdateHostedService>();

builder.Services.AddControllers();

builder.Services.AddCors(options => 
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod()
    )
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
