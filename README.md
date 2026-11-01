# Sistema de tickets internos

Aplicación web para registrar, priorizar y dar seguimiento a tickets de soporte
interno. Hecha para practicar C# y ASP.NET MVC.

## Qué hace

- Crea tickets con prioridad y validación en el servidor.
- Filtra la lista por estatus y prioridad.
- Controla el flujo Abierto → En proceso → Resuelto → Cerrado.
- Rechaza saltos de estatus aunque la petición se arme a mano.
- Guarda un historial de cambios y comentarios.

## Tecnologías

C#, .NET 8, ASP.NET Core MVC, Razor, Entity Framework Core, SQL Server,
Bootstrap, jQuery y xUnit.

## Cómo correrlo

Necesitas el SDK de .NET 8 y SQL Server local con autenticación de Windows.
La base `TicketsDemo` se crea al arrancar.

```bash
dotnet run --project TicketsDemo
```

```bash
dotnet test
```

## Decisiones

- **Reglas fuera del controlador:** `ReglasTicket` se prueba sin base de datos.
- **Validación doble:** la vista solo ofrece cambios válidos, pero quien decide
  es el servidor.
- **`[Bind]` en la creación:** un formulario alterado no puede fijar el estatus
  ni la fecha.
- **Estatus guardados como texto:** se leen directo en SQL Server.
- **`EnsureCreated`:** basta para esta demo. Con datos reales usaría migraciones.

## Cómo se hizo

Lo desarrollé con Claude Code. Verifiqué que compilara sin advertencias, que
pasaran las 9 pruebas y que el flujo funcionara en el navegador, incluidos
datos inválidos, una petición sin token y un salto de estatus no permitido.
