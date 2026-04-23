using Microsoft.EntityFrameworkCore;
using WeatherAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. CORS (Allow All for now)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// 2. Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=/data/weather.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. Ensure Database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// 4. IMPORTANT: Remove HTTPS Redirection (Railway handles SSL)
// app.UseHttpsRedirection(); <-- DELETED OR COMMENTED OUT

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// 5. BIND TO 0.0.0.0 (Crucial for Railway)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");