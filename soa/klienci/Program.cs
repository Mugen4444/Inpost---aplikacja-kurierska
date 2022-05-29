using klienci;
using klienci.Exceptions;

using inpost;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Use Klienci collection as singleton

builder.Services.AddSingleton<Klienci>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// klienci routes

app.MapGet("klienci", (Klienci klienci) =>
{
    return klienci.Wszystkie;
});

app.MapGet("klienci/{id:int}", (int id, Klienci klienci) =>
{
    try
    {
        return Results.Ok(klienci.Klient(id));
    }
    catch (NieZnalezionoKlientaException ex)
    {
        return Results.NotFound(ex.Message);
    }
});

app.MapGet("klienci", ([FromBody] Klient klient, Klienci klienci) =>
{
    try
    {
        return Results.Ok(klienci.Klient(klient));
    }
    catch (NiepoprawneDaneKlientaException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (NieZnalezionoKlientaException ex)
    {
        return Results.NotFound(ex.Message);
    }
});

app.MapPost("klienci", (Klient klient, Klienci klienci) =>
{
    try
    {
        var dodany = klienci.DodajKlienta(klient);
        return Results.Ok(dodany);
    }
    catch (NiepoprawneDaneKlientaException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (NieZnalezionoKlientaException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (KlientOperationException ex)
    {
        return Results.Problem(ex.Message, statusCode: 500);
    }
});

app.MapPut("klienci/{id:int}", (int id, Klient klient, Klienci klienci) =>
{
    try
    {
        var odswiezony = klienci.OdswiezKlienta(id, klient);
        return Results.Ok(odswiezony);
    }
    catch (NiepoprawneDaneKlientaException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (NieZnalezionoKlientaException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (KlientOperationException ex)
    {
        return Results.Problem(ex.Message, statusCode: 500);
    }
});

app.MapDelete("klienci/{id:int}", (int id, Klienci klienci) =>
{
    try
    {
        klienci.UsunKlienta(id);
        return Results.NoContent();
    }
    catch (NieZnalezionoKlientaException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (KlientOperationException ex)
    {
        return Results.Problem(ex.Message, statusCode: 500);
    }
});

app.Run();
