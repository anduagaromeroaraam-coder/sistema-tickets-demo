using Microsoft.EntityFrameworkCore;
using TicketsDemo.Models;

namespace TicketsDemo.Data;

public class TicketsContext(DbContextOptions<TicketsContext> opciones) : DbContext(opciones)
{
    public DbSet<Ticket> Tickets => Set<Ticket>();

    public DbSet<EventoTicket> Eventos => Set<EventoTicket>();

    protected override void OnModelCreating(ModelBuilder modelo)
    {
        // Guardados como texto para que una consulta directa en SQL Server se
        // lea sin tener que recordar qué número era cada estatus.
        modelo.Entity<Ticket>().Property(t => t.Estatus).HasConversion<string>().HasMaxLength(20);
        modelo.Entity<Ticket>().Property(t => t.Prioridad).HasConversion<string>().HasMaxLength(20);

        // La lista filtra por estatus en cada visita.
        modelo.Entity<Ticket>().HasIndex(t => t.Estatus);
    }
}
