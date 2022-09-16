using KebabShop;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<KebabDatabaseContext>
    (options => options.UseInMemoryDatabase("KebabDatabase"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/kebabs", async (KebabDatabaseContext db) =>
    await db.Kebabs.ToListAsync());

app.MapGet("/kebabs/{id:int}", async (int id, KebabDatabaseContext db) =>
    await db.Kebabs.FindAsync(id) is { } kebab // check for null
        ? Results.Ok(kebab) // 200 response
        : Results.NotFound()); // 404 response

app.MapGet("/kebabs/vege", async (KebabDatabaseContext db) =>
    await db.Kebabs.Where(kebab => kebab.IsVege).ToListAsync());

app.MapPost("/kebabs", async (Kebab kebab, KebabDatabaseContext db) =>
{
    db.Kebabs.Add(kebab);
    await db.SaveChangesAsync();

    return Results.Created($"/kebabs/{kebab.Id}", kebab);
});

app.MapPut("/kebabs/{id:int}", async (int id, Kebab inputKebab, KebabDatabaseContext db) =>
{
    var kebabToUpdate = await db.Kebabs.FindAsync(id);

    if (kebabToUpdate is null)
        return Results.NotFound();
    
    // ReSharper disable once RedundantAssignment
    kebabToUpdate = kebabToUpdate with
        { Name = inputKebab.Name, IsVege = inputKebab.IsVege, Price = inputKebab.Price };
    
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/kebabs/{id:int}", async (int id, KebabDatabaseContext db) =>
{
    if (await db.Kebabs.FindAsync(id) is not { } kebab) return Results.NotFound();
    
    db.Kebabs.Remove(kebab);
    await db.SaveChangesAsync();
    return Results.Ok(kebab);
});

app.Run();