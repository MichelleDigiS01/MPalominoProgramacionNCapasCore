using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

//var conString = builder.Configuration.GetConnectionString("MPalominoProgramacionNCapasCore");
//builder.Services.AddDbContext<DL.MpalominoProgramacionNcapasContext>(options => options.UseSqlServer(conString));

//builder.Services.AddScoped<BL.Usuario>();  Usado para la inyeccion de dependencias

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
