using System.ComponentModel.DataAnnotations;

namespace TicketsDemo.Models;

public enum EstatusTicket
{
    Abierto,
    EnProceso,
    Resuelto,
    Cerrado
}

public enum PrioridadTicket
{
    Baja,
    Media,
    Alta
}

public class Ticket
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Escribe un título.")]
    [StringLength(120, MinimumLength = 5, ErrorMessage = "El título lleva de 5 a 120 caracteres.")]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = "";

    [Required(ErrorMessage = "Describe el problema.")]
    [StringLength(2000)]
    [Display(Name = "Descripción")]
    public string Descripcion { get; set; } = "";

    [Required(ErrorMessage = "Indica quién lo reporta.")]
    [StringLength(80)]
    [Display(Name = "Reportado por")]
    public string ReportadoPor { get; set; } = "";

    public PrioridadTicket Prioridad { get; set; } = PrioridadTicket.Media;

    public EstatusTicket Estatus { get; set; } = EstatusTicket.Abierto;

    // La pone el servidor: si viniera del formulario, cualquiera podría
    // antedatar un ticket y mover el orden de la lista.
    public DateTime CreadoEn { get; set; }

    public List<EventoTicket> Eventos { get; set; } = [];
}

/// <summary>
/// Historial de un ticket. Se agrega y nunca se edita: sin ese rastro no se
/// puede saber quién cambió un estatus cuando alguien lo reclama.
/// </summary>
public class EventoTicket
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public Ticket? Ticket { get; set; }

    [Required, StringLength(500)]
    public string Detalle { get; set; } = "";

    public DateTime CreadoEn { get; set; }
}
