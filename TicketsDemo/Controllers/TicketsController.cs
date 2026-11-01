using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketsDemo.Data;
using TicketsDemo.Models;

namespace TicketsDemo.Controllers;

public class TicketsController(TicketsContext bd) : Controller
{
    public async Task<IActionResult> Index(EstatusTicket? estatus, PrioridadTicket? prioridad)
    {
        var consulta = bd.Tickets.AsNoTracking();

        if (estatus is not null)
        {
            consulta = consulta.Where(t => t.Estatus == estatus);
        }

        if (prioridad is not null)
        {
            consulta = consulta.Where(t => t.Prioridad == prioridad);
        }

        ViewBag.Estatus = estatus;
        ViewBag.Prioridad = prioridad;

        // El Id desempata: dos tickets creados a la vez saldrían en orden
        // distinto entre una consulta y otra.
        return View(await consulta.OrderByDescending(t => t.CreadoEn).ThenByDescending(t => t.Id).ToListAsync());
    }

    public IActionResult Crear() => View(new Ticket());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear([Bind("Titulo,Descripcion,ReportadoPor,Prioridad")] Ticket ticket)
    {
        if (!ModelState.IsValid)
        {
            return View(ticket);
        }

        var ahora = DateTime.UtcNow;
        ticket.Estatus = EstatusTicket.Abierto;
        ticket.CreadoEn = ahora;
        ticket.Eventos.Add(new EventoTicket { Detalle = $"Ticket creado por {ticket.ReportadoPor}.", CreadoEn = ahora });

        bd.Tickets.Add(ticket);
        await bd.SaveChangesAsync();

        TempData["Aviso"] = $"Ticket #{ticket.Id} creado.";
        return RedirectToAction(nameof(Detalle), new { id = ticket.Id });
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var ticket = await bd.Tickets
            .AsNoTracking()
            .Include(t => t.Eventos.OrderByDescending(e => e.CreadoEn).ThenByDescending(e => e.Id))
            .FirstOrDefaultAsync(t => t.Id == id);

        return ticket is null ? NotFound() : View(ticket);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstatus(int id, EstatusTicket nuevo)
    {
        var ticket = await bd.Tickets.FindAsync(id);
        if (ticket is null)
        {
            return NotFound();
        }

        // Se valida aquí aunque la vista solo ofrezca los botones válidos: la
        // petición se puede armar a mano y la vista no protege nada.
        if (!ReglasTicket.PuedeCambiar(ticket.Estatus, nuevo))
        {
            TempData["Error"] = $"No se puede pasar de {ReglasTicket.Nombre(ticket.Estatus)} a {ReglasTicket.Nombre(nuevo)}.";
            return RedirectToAction(nameof(Detalle), new { id });
        }

        var detalle = $"Estatus: {ReglasTicket.Nombre(ticket.Estatus)} → {ReglasTicket.Nombre(nuevo)}.";
        ticket.Estatus = nuevo;
        bd.Eventos.Add(new EventoTicket { TicketId = id, Detalle = detalle, CreadoEn = DateTime.UtcNow });
        await bd.SaveChangesAsync();

        TempData["Aviso"] = "Estatus actualizado.";
        return RedirectToAction(nameof(Detalle), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Comentar(int id, string comentario)
    {
        if (string.IsNullOrWhiteSpace(comentario) || comentario.Length > 500)
        {
            TempData["Error"] = "El comentario lleva de 1 a 500 caracteres.";
            return RedirectToAction(nameof(Detalle), new { id });
        }

        if (!await bd.Tickets.AnyAsync(t => t.Id == id))
        {
            return NotFound();
        }

        bd.Eventos.Add(new EventoTicket { TicketId = id, Detalle = comentario.Trim(), CreadoEn = DateTime.UtcNow });
        await bd.SaveChangesAsync();
        return RedirectToAction(nameof(Detalle), new { id });
    }
}
