using System;
using System.Collections.Generic;
class Navegador 
{
    private Stack<string> historial;

    public string PaginaActual { get; private set; } 
    public Navegador()
    {
        historial = new Stack<string>();
        PaginaActual = "Pagina de inicio";

    }
    public void Visitar(string nuevaPagina)
    {
        historial.Push(PaginaActual);
        PaginaActual = nuevaPagina;
    }
    public bool Retroceder()
    {
        if (historial.Count ==0)
        {
            return false;
        }
        PaginaActual = historial.Pop();
        return true;
    
    }
    public void MostrarHistorial()
    {
        Console.WriteLine("\n=====HISTORIAL DE PÁGINAS=====:");
        Console.WriteLine($"Página actual: {PaginaActual}");
        if (historial.Count == 0)
        {
            Console.WriteLine("No hay páginas anteriores en el historial.");
            
        }
        else
        {
            Console.WriteLine("Páginas anteriores en el historial:");
        }

        foreach (string paginaGuardada in historial)
        {
            Console.WriteLine($"- {paginaGuardada}");
        }
        Console.WriteLine($"Cantidad de páginas guardadas: {historial.Count}");
    }

} 