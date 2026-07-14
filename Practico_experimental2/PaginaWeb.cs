using System;

class PaginaWeb
{
    public string Titulo { get; private set; }
    public string Url { get; private set; }
    public string Categoria { get; private set; }
    public DateTime FechaVisita { get; private set; }

    public PaginaWeb(string titulo, string url, string categoria)
    {
        Titulo = titulo;
        Url = url;
        Categoria = categoria;
        FechaVisita = DateTime.Now;
    }

    public void MostrarDetalle()
    {
        Console.WriteLine($"Título: {Titulo}");
        Console.WriteLine($"URL: {Url}");
        Console.WriteLine($"Categoría: {Categoria}");
        Console.WriteLine($"Fecha de visita: {FechaVisita:dd/MM/yyyy HH:mm:ss}");
        Console.WriteLine(new string('-', 45));
    }
}
