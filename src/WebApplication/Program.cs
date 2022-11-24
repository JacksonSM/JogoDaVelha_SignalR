using Microsoft.EntityFrameworkCore;
using WebApplication_Jogo.DataBase;
using WebApplication_Jogo.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

builder.Services.AddScoped<PartidaRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(cfg => {
    cfg.UseSqlite("Data Source=Database\\Partidas.db");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


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
