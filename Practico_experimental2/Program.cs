using System;
using System.Collections.Generic;

Navegador navegador = new Navegador();
int opcion = 0;

do
{
    Console.WriteLine("\n===== NAVEGADOR WEB =====");
    Console.WriteLine($"Página actual: {navegador.PaginaActual.Titulo}");
    Console.WriteLine("1. Visitar una nueva página");
    Console.WriteLine("2. Retroceder a la página anterior");
    Console.WriteLine("3. Mostrar historial para retroceder");
    Console.WriteLine("4. Mostrar reporte completo de visitas");
    Console.WriteLine("5. Consultar una página en el historial de retroceso");
    Console.WriteLine("6. Salir");
    Console.Write("Seleccione una opción: ");

    if (!int.TryParse(Console.ReadLine(), out opcion))
    {
        Console.WriteLine("Debe ingresar un número.");
        continue;
    }

    switch (opcion)
    {
        case 1:
            Console.Write("Ingrese el título de la página: ");
            string titulo = Console.ReadLine() ?? "";
            Console.Write("Ingrese la URL: ");
            string url = Console.ReadLine() ?? "";
            Console.Write("Ingrese la categoría: ");
            string categoria = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(titulo) ||
                string.IsNullOrWhiteSpace(url) ||
                string.IsNullOrWhiteSpace(categoria))
            {
                Console.WriteLine("Ningún dato puede quedar vacío.");
                break;
            }

            PaginaWeb nuevaPagina = new PaginaWeb(titulo, url, categoria);
            navegador.Visitar(nuevaPagina);
            Console.WriteLine($"Registro agregado. Visitando: {navegador.PaginaActual.Titulo}");
            break;

        case 2:
            if (navegador.Retroceder())
            {
                Console.WriteLine($"Regresó a: {navegador.PaginaActual.Titulo}");
            }
            else
            {
                Console.WriteLine("No hay páginas anteriores en el historial.");
            }
            break;

        case 3:
            navegador.MostrarHistorialRetroceso();
            break;

        case 4:
            navegador.MostrarReporteVisitas();
            break;

        case 5:
            Console.Write("Ingrese título, URL o categoría para consultar la pila: ");
            string textoBusqueda = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(textoBusqueda))
            {
                Console.WriteLine("El texto de búsqueda no puede estar vacío.");
                break;
            }

            List<PaginaWeb> resultados = navegador.BuscarEnHistorialRetroceso(textoBusqueda);
            Console.WriteLine($"\n===== CONSULTA EN LA PILA: {resultados.Count} RESULTADO(S) =====");

            if (resultados.Count == 0)
            {
                Console.WriteLine("No se encontraron páginas que coincidan.");
            }
            else
            {
                foreach (PaginaWeb pagina in resultados)
                {
                    pagina.MostrarDetalle();
                }
            }
            break;

        case 6:
            Console.WriteLine("Saliendo del programa...");
            break;

        default:
            Console.WriteLine("Opción inválida. Intente nuevamente.");
            break;
    }
} while (opcion != 6);
