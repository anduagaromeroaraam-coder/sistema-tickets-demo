namespace TicketsDemo.Models;

/// <summary>
/// Las transiciones viven aquí y no en el controlador para poder probarlas sin
/// base de datos ni peticiones HTTP.
/// </summary>
public static class ReglasTicket
{
    private static readonly Dictionary<EstatusTicket, EstatusTicket[]> Permitidas = new()
    {
        [EstatusTicket.Abierto] = [EstatusTicket.EnProceso],
        [EstatusTicket.EnProceso] = [EstatusTicket.Resuelto, EstatusTicket.Abierto],
        [EstatusTicket.Resuelto] = [EstatusTicket.Cerrado, EstatusTicket.EnProceso],
        // Cerrado no tiene salida: si el fallo vuelve, es un ticket nuevo y el
        // tiempo de atención del primero no se altera.
        [EstatusTicket.Cerrado] = []
    };

    public static IReadOnlyList<EstatusTicket> Siguientes(EstatusTicket actual) => Permitidas[actual];

    public static bool PuedeCambiar(EstatusTicket actual, EstatusTicket nuevo) =>
        Permitidas[actual].Contains(nuevo);

    public static string Nombre(EstatusTicket estatus) => estatus switch
    {
        EstatusTicket.Abierto => "Abierto",
        EstatusTicket.EnProceso => "En proceso",
        EstatusTicket.Resuelto => "Resuelto",
        EstatusTicket.Cerrado => "Cerrado",
        _ => estatus.ToString()
    };
}
