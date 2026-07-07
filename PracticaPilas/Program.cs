using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Stack<string> historial = new Stack<string>();
        int opcion;

        do
        {
            Console.Clear();

            Console.WriteLine("=================================");
            Console.WriteLine("      NAVEGADOR WEB");
            Console.WriteLine("=================================");
            Console.WriteLine("1. Visitar una página");
            Console.WriteLine("2. Retroceder");
            Console.WriteLine("3. Mostrar historial");
            Console.WriteLine("4. Salir");
            Console.Write("\nSeleccione una opción: ");

            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                Console.WriteLine("Opción inválida.");
                Console.WriteLine("Presione una tecla para continuar...");
                Console.ReadKey();
                continue;
            }

            switch (opcion)
            {
                case 1:
                    Console.Write("\nIngrese la página web: ");
                    string pagina = Console.ReadLine() ?? "";

                    if (pagina != "")
                    {
                        historial.Push(pagina);
                        Console.WriteLine("Página agregada correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("No ingresó ninguna página.");
                    }
                    break;

                case 2:
                    if (historial.Count > 0)
                    {
                        string anterior = historial.Pop();
                        Console.WriteLine($"\nRetrocediendo desde: {anterior}");
                    }
                    else
                    {
                        Console.WriteLine("\nNo hay páginas para retroceder.");
                    }
                    break;

                case 3:
                    Console.WriteLine("\n===== HISTORIAL =====");

                    if (historial.Count == 0)
                    {
                        Console.WriteLine("No existen páginas en el historial.");
                    }
                    else
                    {
                        foreach (string item in historial)
                        {
                            Console.WriteLine(item);
                        }

                        Console.WriteLine($"\nTotal de páginas: {historial.Count}");
                    }
                    break;

                case 4:
                    Console.WriteLine("\nGracias por utilizar el sistema.");
                    break;

                default:
                    Console.WriteLine("\nOpción incorrecta.");
                    break;
            }

            if (opcion != 4)
            {
                Console.WriteLine("\nPresione una tecla para continuar...");
                Console.ReadKey();
            }

        } while (opcion != 4);
    }
}