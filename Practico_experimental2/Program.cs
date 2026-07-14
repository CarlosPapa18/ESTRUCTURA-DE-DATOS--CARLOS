using System;

Navegador navegador = new Navegador();
int opcion = 0;

do
{
    Console.WriteLine("\n=====NAVEGADOR WEB=====");
    Console.WriteLine($"Página actual: {navegador.PaginaActual}");
    Console.WriteLine("1. Visitar una nueva página");
    Console.WriteLine("2. Retroceder a la página anterior");
    Console.WriteLine("3. Mostrar historial de páginas");
    Console.WriteLine("4. Salir");
    Console.Write("Seleccione una opción: ");

    if (!int.TryParse(Console.ReadLine(), out opcion))
    {
        Console.WriteLine("Debe ingresar un número.");
        continue;
    }
    
    switch (opcion)
    {
        case 1:
            Console.Write("Ingrese la URL de la nueva página: ");
            string nuevaPagina = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(nuevaPagina))
            {
                Console.WriteLine("La URL no puede estar vacía. Intente nuevamente.");
                break;
            }
            else 
            {
            
                navegador.Visitar(nuevaPagina);
                Console.WriteLine($"Visitando: {navegador.PaginaActual}");
            }
            break;

        case 2:
            if (navegador.Retroceder())
            {
                Console.WriteLine($"Regresó a: {navegador.PaginaActual}");
            }
            else
            {    
                Console.WriteLine("No hay páginas anteriores en el historial.");
            }
            break;
        case 3:
            navegador.MostrarHistorial();
            break;
        case 4:
            Console.WriteLine("Saliendo del programa...");
            break;
        default:
            Console.WriteLine("Opción inválida. Intente nuevamente.");
            break;
    }
} while (opcion != 4);