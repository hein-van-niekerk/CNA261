using DemoApi.Data;
using DemoApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () =>
    Results.Content(
        """
        <!doctype html>
        <html>
          <head><meta charset="utf-8"><title>DemoApi</title></head>
          <body>
            <h1>DemoApi</h1>
            <ul>
              <li><a href="/weather">/weather</a></li>
              <li><a href="/db-status">/db-status</a></li>
              <li><a href="/swagger">/swagger</a></li>
            </ul>
          </body>
        </html>
        """,
        "text/html"));

app.MapGet("/weather", async (AppDbContext db) =>
{
    if (!await db.WeatherRecords.AnyAsync())
    {
        db.WeatherRecords.Add(new WeatherRecord
        {
            Summary = "Seeded weather row",
            TemperatureC = 22
        });

        await db.SaveChangesAsync();
    }

    var records = await db.WeatherRecords.ToListAsync();
    return Results.Ok(records);
});

  app.MapGet("/db-status", async (AppDbContext db) =>
  {
    var canConnect = await db.Database.CanConnectAsync();
    return Results.Ok(new
    {
      connected = canConnect,
      checkedAtUtc = DateTime.UtcNow
    });
  });

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();