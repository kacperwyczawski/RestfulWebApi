using KebabShop;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSqlite<KebabContext>("Data Source=Kebab.db");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/kebabs", async (KebabContext db) =>
    await db.Kebabs
        .AsNoTracking()
        .ToListAsync());

app.MapGet("/kebabs/{id:int}", async (int id, KebabContext db) =>
    await db.Kebabs
        .AsNoTracking()
        .FirstOrDefaultAsync(k => k.Id == id));

app.MapGet("/kebabs/vege", async (KebabContext db) =>
    await db.Kebabs
        .AsNoTracking()
        .Where(k => k.IsVege)
        .ToListAsync());

app.MapPost("/kebabs", async (Kebab kebab, KebabContext db) =>
{
    db.Kebabs.Add(kebab);
    await db.SaveChangesAsync();
    
    return Results.Created($"/kebabs/{kebab.Id}", kebab);
});

app.MapPut("/kebabs/{id:int}", async (int id, Kebab inputKebab, KebabContext db) =>
{
    var kebabToUpdate = await db.Kebabs.FindAsync(id);
        
    if (kebabToUpdate is null)
        return Results.NotFound();

    kebabToUpdate.Name = inputKebab.Name;
    kebabToUpdate.IsVege = inputKebab.IsVege;
    kebabToUpdate.Price = inputKebab.Price;

    db.Kebabs.Update(kebabToUpdate);
    await db.SaveChangesAsync();
    
    return Results.NoContent();
});

app.MapDelete("/kebabs/{id:int}", async (int id, KebabContext db) =>
{
    var kebabToDelete = await db.Kebabs.FindAsync(id);
    
    if (kebabToDelete is null)
        return Results.NotFound();
    
    db.Kebabs.Remove(kebabToDelete);
    await db.SaveChangesAsync();
    
    return Results.NoContent();
});

app.Run();