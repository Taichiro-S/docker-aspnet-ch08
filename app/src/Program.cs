using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Default")
    ));

var app = builder.Build();

app.UseDefaultFiles(); // index.html
app.UseStaticFiles();  // wwwroot

// app.MapControllers();

app.MapGet("/api/health", async (AppDbContext db) =>
{
    try
    {
        var canConnect = await db.Database.CanConnectAsync();

        if (canConnect)
        {
            return Results.Ok(new
            {
                status = "OK",
                message = "App → DB connection successful"
            });
        }
        else
        {
            return Results.Problem("DB connection failed");
        }
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Run();
