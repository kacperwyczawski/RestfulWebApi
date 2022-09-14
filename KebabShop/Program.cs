using KebabShop;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<KebabDatabaseContext>
    (options => options.UseInMemoryDatabase("KebabDatabase"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/kebabs", async (KebabDatabaseContext db) =>
    await db.Kebabs.ToListAsync());

app.MapGet("/kebabs/{id}", async (int id, KebabDatabaseContext db) =>
    await db.Kebabs.FindAsync(id) is { } kebab // check for null
        ? Results.Ok(kebab) // 200 response
        : Results.NotFound()); // 404 response

app.MapGet("/kebabs/vege", async (KebabDatabaseContext db) =>
    await db.Kebabs.Where(kebab => kebab.IsVege).ToListAsync());

app.Run();