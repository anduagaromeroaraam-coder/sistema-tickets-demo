using TicketsDemo.Models;
using Xunit;

namespace TicketsDemo.Pruebas;

public class ReglasTicketPruebas
{
    [Theory]
    [InlineData(EstatusTicket.Abierto, EstatusTicket.EnProceso)]
    [InlineData(EstatusTicket.EnProceso, EstatusTicket.Resuelto)]
    [InlineData(EstatusTicket.EnProceso, EstatusTicket.Abierto)]
    [InlineData(EstatusTicket.Resuelto, EstatusTicket.Cerrado)]
    [InlineData(EstatusTicket.Resuelto, EstatusTicket.EnProceso)]
    public void Permite_las_transiciones_del_flujo(EstatusTicket actual, EstatusTicket nuevo)
    {
        Assert.True(ReglasTicket.PuedeCambiar(actual, nuevo));
    }

    [Theory]
    [InlineData(EstatusTicket.Abierto, EstatusTicket.Cerrado)]
    [InlineData(EstatusTicket.Abierto, EstatusTicket.Resuelto)]
    [InlineData(EstatusTicket.Cerrado, EstatusTicket.Abierto)]
    public void Rechaza_saltos_y_reabrir_un_cerrado(EstatusTicket actual, EstatusTicket nuevo)
    {
        Assert.False(ReglasTicket.PuedeCambiar(actual, nuevo));
    }

    [Fact]
    public void Un_ticket_cerrado_no_ofrece_siguientes_estatus()
    {
        Assert.Empty(ReglasTicket.Siguientes(EstatusTicket.Cerrado));
    }
}
