using System;
using System.Collections.Generic;

class Navegador
{
    private Stack<PaginaWeb> historialRetroceso;
    private List<PaginaWeb> registroVisitas;

    public PaginaWeb PaginaActual { get; private set; }

    public Navegador()
    {
        historialRetroceso = new Stack<PaginaWeb>();
        registroVisitas = new List<PaginaWeb>();
        PaginaActual = new PaginaWeb("Página de inicio", "inicio://local", "Sistema");
    }

    public void Visitar(PaginaWeb nuevaPagina)
    {
        historialRetroceso.Push(PaginaActual);
        PaginaActual = nuevaPagina;
        registroVisitas.Add(nuevaPagina);
    }

    public bool Retroceder()
    {
        if (historialRetroceso.Count == 0)
        {
            return false;
        }

        PaginaActual = historialRetroceso.Pop();
        return true;
    }

    public void MostrarHistorialRetroceso()
    {
        Console.WriteLine("\n===== HISTORIAL PARA RETROCEDER =====");
        Console.WriteLine($"Página actual: {PaginaActual.Titulo}");

        if (historialRetroceso.Count == 0)
        {
            Console.WriteLine("No hay páginas anteriores en el historial.");
            return;
        }

        foreach (PaginaWeb pagina in historialRetroceso)
        {
            Console.WriteLine($"- {pagina.Titulo} | {pagina.Url}");
        }

        Console.WriteLine($"Total disponible para retroceder: {historialRetroceso.Count}");
    }

    public void MostrarReporteVisitas()
    {
        Console.WriteLine("\n===== REPORTE COMPLETO DE VISITAS =====");

        if (registroVisitas.Count == 0)
        {
            Console.WriteLine("No se han registrado páginas visitadas.");
            return;
        }

        foreach (PaginaWeb pagina in registroVisitas)
        {
            pagina.MostrarDetalle();
        }

        Console.WriteLine($"Total de páginas visitadas: {registroVisitas.Count}");
    }

    public List<PaginaWeb> BuscarEnHistorialRetroceso(string texto)
    {
        List<PaginaWeb> resultados = new List<PaginaWeb>();

        foreach (PaginaWeb pagina in historialRetroceso)
        {
            bool coincideTitulo = pagina.Titulo.Contains(texto, StringComparison.OrdinalIgnoreCase);
            bool coincideUrl = pagina.Url.Contains(texto, StringComparison.OrdinalIgnoreCase);
            bool coincideCategoria = pagina.Categoria.Contains(texto, StringComparison.OrdinalIgnoreCase);

            if (coincideTitulo || coincideUrl || coincideCategoria)
            {
                resultados.Add(pagina);
            }
        }

        return resultados;
    }
}
