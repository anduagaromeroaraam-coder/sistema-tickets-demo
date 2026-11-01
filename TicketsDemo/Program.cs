using Microsoft.EntityFrameworkCore;
using TicketsDemo.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TicketsContext>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("Tickets")));

var app = builder.Build();

// EnsureCreated basta para una demo local que se puede tirar y rehacer. Un
// sistema con datos de verdad necesitaría migraciones.
using (var alcance = app.Services.CreateScope())
{
    alcance.ServiceProvider.GetRequiredService<TicketsContext>().Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Tickets");
}

app.UseRouting();
app.MapControllerRoute("predeterminada", "{controller=Tickets}/{action=Index}/{id?}");

app.Run();

// Lo necesita el proyecto de pruebas para referirse a esta aplicación.
public partial class Program;
