using mvc.Models.Magazyn;
using mvc.Models;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Use MVC models as scoped

builder.Services.AddScoped<PaczkaViewModel>();
builder.Services.AddSingleton<ErrorViewModel>();

// Add Http clients

builder.Services.AddHttpClient("Magazyn", client =>
{
    var magazynUrl = builder.Configuration.GetSection("magazynUrl");

    if (string.IsNullOrEmpty(magazynUrl.Value))
        throw new Exception("Brak linku do usługi \"magazyn\" (klucz \"magazynUrl\" w appsettings.json");

    client.BaseAddress = new Uri(magazynUrl.Value);

    client.DefaultRequestHeaders.Add(
        HeaderNames.Accept, "application/json");
    client.DefaultRequestHeaders.Add(
        HeaderNames.UserAgent, "inpost");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}


app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
