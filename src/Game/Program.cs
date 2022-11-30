using Game.Context;
using Game.Filter;
using Game.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddSignalR(options =>
{
    options.AddFilter<ExceptionFilter>();
});

builder.Services.AddScoped<PartidaRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(cfg => {
    cfg.UseInMemoryDatabase("Partidas");
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<PartidaHub>("/jogo-da-velha-hub");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
