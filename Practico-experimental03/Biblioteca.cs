using System.Text.Json;

namespace PracticoExperimental03;

public sealed class Biblioteca
{
    // Diccionario maestro: una clave ISBN identifica un solo libro.
    private readonly Dictionary<string, Libro> catalogo = new(StringComparer.OrdinalIgnoreCase);

    // Mapas invertidos: una clave de consulta se relaciona con varios ISBN únicos.
    private readonly Dictionary<string, HashSet<string>> librosPorAutor =
        new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, HashSet<string>> librosPorCategoria =
        new(StringComparer.OrdinalIgnoreCase);

    // Conjunto para comprobar rápidamente si un ISBN está disponible.
    private readonly HashSet<string> disponibles = new(StringComparer.OrdinalIgnoreCase);

    // Diccionario que relaciona cada ISBN prestado con el lector responsable.
    private readonly Dictionary<string, string> prestamos = new(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyDictionary<string, Libro> Catalogo => catalogo;
    public IReadOnlySet<string> Disponibles => disponibles;
    public IReadOnlyDictionary<string, string> Prestamos => prestamos;

    private static string Normalizar(string texto) =>
        string.Join(' ', texto.Trim().ToLowerInvariant()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries));

    private static void AgregarAIndice(
        Dictionary<string, HashSet<string>> indice,
        string clave,
        string isbn)
    {
        if (!indice.TryGetValue(clave, out HashSet<string>? codigos))
        {
            codigos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            indice[clave] = codigos;
        }

        codigos.Add(isbn);
    }

    private static void QuitarDeIndice(
        Dictionary<string, HashSet<string>> indice,
        string clave,
        string isbn)
    {
        if (!indice.TryGetValue(clave, out HashSet<string>? codigos))
            return;

        codigos.Remove(isbn);
        if (codigos.Count == 0)
            indice.Remove(clave);
    }

    public bool RegistrarLibro(Libro libro, out string mensaje)
    {
        if (string.IsNullOrWhiteSpace(libro.Isbn) ||
            string.IsNullOrWhiteSpace(libro.Titulo) ||
            string.IsNullOrWhiteSpace(libro.Autor) ||
            string.IsNullOrWhiteSpace(libro.Categoria) ||
            libro.Anio <= 0)
        {
            mensaje = "Los datos del libro son incompletos o inválidos.";
            return false;
        }

        if (!catalogo.TryAdd(libro.Isbn.Trim(), libro))
        {
            mensaje = $"El ISBN {libro.Isbn} ya se encuentra registrado.";
            return false;
        }

        disponibles.Add(libro.Isbn);
        AgregarAIndice(librosPorAutor, Normalizar(libro.Autor), libro.Isbn);
        AgregarAIndice(librosPorCategoria, Normalizar(libro.Categoria), libro.Isbn);
        mensaje = "Libro registrado correctamente.";
        return true;
    }

    public bool EliminarLibro(string isbn, out string mensaje)
    {
        if (prestamos.ContainsKey(isbn))
        {
            mensaje = "No se puede eliminar un libro que está prestado.";
            return false;
        }

        if (!catalogo.Remove(isbn, out Libro? libro))
        {
            mensaje = "No existe un libro con ese ISBN.";
            return false;
        }

        disponibles.Remove(isbn);
        QuitarDeIndice(librosPorAutor, Normalizar(libro.Autor), isbn);
        QuitarDeIndice(librosPorCategoria, Normalizar(libro.Categoria), isbn);
        mensaje = "Libro eliminado correctamente.";
        return true;
    }

    public Libro? BuscarPorIsbn(string isbn) =>
        catalogo.GetValueOrDefault(isbn);

    public IReadOnlyList<Libro> BuscarPorAutor(string autor) =>
        BuscarEnIndice(librosPorAutor, Normalizar(autor));

    public IReadOnlyList<Libro> BuscarPorCategoria(string categoria) =>
        BuscarEnIndice(librosPorCategoria, Normalizar(categoria));

    private IReadOnlyList<Libro> BuscarEnIndice(
        Dictionary<string, HashSet<string>> indice,
        string clave)
    {
        if (!indice.TryGetValue(clave, out HashSet<string>? codigos))
            return [];

        return codigos.Select(isbn => catalogo[isbn])
            .OrderBy(libro => libro.Titulo)
            .ToList();
    }

    public bool PrestarLibro(string isbn, string lector, out string mensaje)
    {
        if (!catalogo.ContainsKey(isbn))
        {
            mensaje = "No existe un libro con ese ISBN.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(lector))
        {
            mensaje = "El nombre del lector es obligatorio.";
            return false;
        }

        if (!disponibles.Remove(isbn))
        {
            mensaje = "El libro no está disponible.";
            return false;
        }

        prestamos[isbn] = lector.Trim();
        mensaje = "Préstamo registrado correctamente.";
        return true;
    }

    public bool DevolverLibro(string isbn, out string mensaje)
    {
        if (!prestamos.Remove(isbn))
        {
            mensaje = "El libro no registra un préstamo activo.";
            return false;
        }

        disponibles.Add(isbn);
        mensaje = "Devolución registrada correctamente.";
        return true;
    }

    public void MostrarReporte()
    {
        Console.WriteLine("\n================ CATÁLOGO DE LA BIBLIOTECA ================");
        Console.WriteLine($"{"ISBN",-15} {"TÍTULO",-28} {"CATEGORÍA",-18} {"ESTADO",-12} LECTOR");
        Console.WriteLine(new string('-', 95));

        foreach (Libro libro in catalogo.Values
                     .OrderBy(libro => libro.Categoria)
                     .ThenBy(libro => libro.Titulo))
        {
            string estado = disponibles.Contains(libro.Isbn) ? "Disponible" : "Prestado";
            string lector = prestamos.GetValueOrDefault(libro.Isbn, "-");
            Console.WriteLine($"{Recortar(libro.Isbn, 15),-15} " +
                              $"{Recortar(libro.Titulo, 28),-28} " +
                              $"{Recortar(libro.Categoria, 18),-18} " +
                              $"{estado,-12} {lector}");
        }

        Console.WriteLine(new string('-', 95));
        Console.WriteLine($"Total: {catalogo.Count} | Disponibles: {disponibles.Count} | " +
                          $"Prestados: {prestamos.Count} | Autores: {librosPorAutor.Count} | " +
                          $"Categorías: {librosPorCategoria.Count}");
    }

    private static string Recortar(string texto, int longitud) =>
        texto.Length <= longitud ? texto : texto[..(longitud - 3)] + "...";

    public void Guardar(string ruta)
    {
        DatosBiblioteca datos = new(catalogo.Values.ToList(), new Dictionary<string, string>(prestamos));
        string json = JsonSerializer.Serialize(datos, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(ruta, json);
    }

    public static Biblioteca Cargar(string ruta)
    {
        string json = File.ReadAllText(ruta);
        DatosBiblioteca datos = JsonSerializer.Deserialize<DatosBiblioteca>(json)
            ?? throw new InvalidDataException("El archivo no contiene datos válidos.");

        Biblioteca biblioteca = new();
        foreach (Libro libro in datos.Libros)
            biblioteca.RegistrarLibro(libro, out _);
        foreach ((string isbn, string lector) in datos.Prestamos)
            biblioteca.PrestarLibro(isbn, lector, out _);
        return biblioteca;
    }

    private sealed record DatosBiblioteca(
        List<Libro> Libros,
        Dictionary<string, string> Prestamos);
}
