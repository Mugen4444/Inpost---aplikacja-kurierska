using Microsoft.Net.Http.Headers;

using magazyn;
using magazyn.Exceptions;

using inpost;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Use Magazyn collection as singleton

builder.Services.AddSingleton<Magazyn>();

// Add Http clients

builder.Services.AddHttpClient("Klienci", client =>
{
    var magazynUrl = builder.Configuration.GetSection("klienciUrl");

    if (string.IsNullOrEmpty(magazynUrl.Value))
        throw new Exception("Brak linku do usługi \"klienci\" (klucz \"klienciUrl\" w appsettings.json");

    client.BaseAddress = new Uri(magazynUrl.Value);

    client.DefaultRequestHeaders.Add(
        HeaderNames.Accept, "application/json");
    client.DefaultRequestHeaders.Add(
        HeaderNames.UserAgent, "inpost");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// magazyn routes

app.MapGet("paczki", (Magazyn magazyn) =>
{
    return magazyn.Wszystkie;
});

app.MapGet("paczki/{id:int}", (int id, Magazyn magazyn) =>
{
    try
    {
        return Results.Ok(magazyn.Paczka(id));
    }
    catch (NieZnalezionoPaczkiException ex)
    {
        return Results.NotFound(ex.Message);
    }
});

app.MapPost("paczki", async (Paczka paczka, Magazyn magazyn) =>
{
    try
    {
        var dodana = await magazyn.DodajPaczke(paczka);
        return Results.Ok(dodana);
    }
    catch (NiepoprawneDanePaczkiException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (NieZnalezionoPaczkiException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (PaczkaOperationException ex)
    {
        return Results.Problem(ex.Message, statusCode: 500);
    }
});

app.MapPut("paczki/{id:int}", async (int id, Paczka paczka, Magazyn magazyn) =>
{
    try
    {
        var odswiezona = await magazyn.OdswiezPaczke(id, paczka);
        return Results.Ok(odswiezona);
    }
    catch (NiepoprawneDanePaczkiException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (NieZnalezionoPaczkiException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (PaczkaOperationException ex)
    {
        return Results.Problem(ex.Message, statusCode: 500);
    }
});

app.MapDelete("paczki/{id:int}", (int id, Magazyn magazyn) =>
{
    try
    {
        magazyn.UsunPaczke(id);
        return Results.NoContent();
    }
    catch (NieZnalezionoPaczkiException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (PaczkaOperationException ex)
    {
        return Results.Problem(ex.Message, statusCode: 500);
    }
});

app.Run();
