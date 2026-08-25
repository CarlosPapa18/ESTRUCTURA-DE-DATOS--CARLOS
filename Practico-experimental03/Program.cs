using System.Diagnostics;
using PracticoExperimental03;

Console.OutputEncoding = System.Text.Encoding.UTF8;

if (args.Contains("--demo"))
{
    EjecutarDemostracion();
    return;
}

if (args.Contains("--pruebas"))
{
    EjecutarPruebas();
    return;
}

if (args.Contains("--benchmark"))
{
    EjecutarBenchmark();
    return;
}

Biblioteca biblioteca = CrearBibliotecaInicial();
int opcion;

do
{
    Console.WriteLine("\n========== SISTEMA DE BIBLIOTECA ==========");
    Console.WriteLine("1. Registrar libro");
    Console.WriteLine("2. Buscar por ISBN");
    Console.WriteLine("3. Buscar por autor");
    Console.WriteLine("4. Buscar por categoría");
    Console.WriteLine("5. Prestar libro");
    Console.WriteLine("6. Devolver libro");
    Console.WriteLine("7. Mostrar reporte completo");
    Console.WriteLine("8. Eliminar libro");
    Console.WriteLine("9. Guardar datos en JSON");
    Console.WriteLine("0. Salir");
    Console.Write("Seleccione una opción: ");

    if (!int.TryParse(Console.ReadLine(), out opcion))
    {
        Console.WriteLine("Debe ingresar un número.");
        continue;
    }

    switch (opcion)
    {
        case 1:
            RegistrarDesdeConsola(biblioteca);
            break;
        case 2:
            Console.Write("ISBN: ");
            Libro? libro = biblioteca.BuscarPorIsbn(Console.ReadLine() ?? "");
            Console.WriteLine(libro is null ? "No se encontró el libro." : libro);
            break;
        case 3:
            Console.Write("Autor: ");
            MostrarResultados(biblioteca.BuscarPorAutor(Console.ReadLine() ?? ""));
            break;
        case 4:
            Console.Write("Categoría: ");
            MostrarResultados(biblioteca.BuscarPorCategoria(Console.ReadLine() ?? ""));
            break;
        case 5:
            Console.Write("ISBN: ");
            string isbnPrestamo = Console.ReadLine() ?? "";
            Console.Write("Nombre del lector: ");
            biblioteca.PrestarLibro(isbnPrestamo, Console.ReadLine() ?? "", out string mensajePrestamo);
            Console.WriteLine(mensajePrestamo);
            break;
        case 6:
            Console.Write("ISBN: ");
            biblioteca.DevolverLibro(Console.ReadLine() ?? "", out string mensajeDevolucion);
            Console.WriteLine(mensajeDevolucion);
            break;
        case 7:
            biblioteca.MostrarReporte();
            break;
        case 8:
            Console.Write("ISBN: ");
            biblioteca.EliminarLibro(Console.ReadLine() ?? "", out string mensajeEliminar);
            Console.WriteLine(mensajeEliminar);
            break;
        case 9:
            biblioteca.Guardar("biblioteca.json");
            Console.WriteLine("Datos guardados en biblioteca.json.");
            break;
        case 0:
            Console.WriteLine("Programa finalizado.");
            break;
        default:
            Console.WriteLine("Opción no válida.");
            break;
    }
} while (opcion != 0);

static Biblioteca CrearBibliotecaInicial()
{
    Biblioteca biblioteca = new();
    Libro[] libros =
    [
        new("9780134685991", "Effective Java", "Joshua Bloch", "Programación", 2018),
        new("9781492056355", "Fluent Python", "Luciano Ramalho", "Programación", 2022),
        new("9780262046305", "Introduction to Algorithms", "Thomas H. Cormen", "Algoritmos", 2022),
        new("9780134757599", "Refactoring", "Martin Fowler", "Ingeniería de software", 2018),
        new("9788417347444", "El infinito en un junco", "Irene Vallejo", "Ensayo", 2019)
    ];

    foreach (Libro libro in libros)
        biblioteca.RegistrarLibro(libro, out _);
    return biblioteca;
}

static void RegistrarDesdeConsola(Biblioteca biblioteca)
{
    Console.Write("ISBN: ");
    string isbn = Console.ReadLine() ?? "";
    Console.Write("Título: ");
    string titulo = Console.ReadLine() ?? "";
    Console.Write("Autor: ");
    string autor = Console.ReadLine() ?? "";
    Console.Write("Categoría: ");
    string categoria = Console.ReadLine() ?? "";
    Console.Write("Año: ");
    int.TryParse(Console.ReadLine(), out int anio);
    biblioteca.RegistrarLibro(new Libro(isbn, titulo, autor, categoria, anio), out string mensaje);
    Console.WriteLine(mensaje);
}

static void MostrarResultados(IReadOnlyList<Libro> libros)
{
    if (libros.Count == 0)
    {
        Console.WriteLine("No se encontraron coincidencias.");
        return;
    }

    foreach (Libro libro in libros)
        Console.WriteLine(libro);
}

static void EjecutarDemostracion()
{
    Biblioteca biblioteca = CrearBibliotecaInicial();
    Console.WriteLine("DEMOSTRACIÓN REPRODUCIBLE - PRÁCTICA 03");
    biblioteca.MostrarReporte();

    biblioteca.PrestarLibro("9781492056355", "Ana López", out string prestamo);
    Console.WriteLine($"\nPréstamo: {prestamo}");
    Console.WriteLine("Consulta por categoría Programación:");
    MostrarResultados(biblioteca.BuscarPorCategoria("programación"));

    biblioteca.PrestarLibro("9781492056355", "Carlos Ruiz", out string duplicado);
    Console.WriteLine($"Validación de préstamo duplicado: {duplicado}");

    biblioteca.DevolverLibro("9781492056355", out string devolucion);
    Console.WriteLine($"Devolución: {devolucion}");
    biblioteca.MostrarReporte();
}

static void EjecutarPruebas()
{
    int aprobadas = 0;
    Probar("Registro e índice por autor", () =>
    {
        Biblioteca b = CrearBibliotecaInicial();
        return b.BuscarPorAutor("  JOSHUA   BLOCH ").Count == 1;
    }, ref aprobadas);
    Probar("Rechazo de ISBN duplicado", () =>
    {
        Biblioteca b = CrearBibliotecaInicial();
        return !b.RegistrarLibro(new Libro("9780134685991", "Otro", "Autor", "Tema", 2024), out _);
    }, ref aprobadas);
    Probar("Préstamo y devolución", () =>
    {
        Biblioteca b = CrearBibliotecaInicial();
        bool prestar = b.PrestarLibro("9781492056355", "Ana", out _);
        bool devolver = b.DevolverLibro("9781492056355", out _);
        return prestar && devolver && b.Disponibles.Contains("9781492056355");
    }, ref aprobadas);
    Probar("Bloqueo de préstamo duplicado", () =>
    {
        Biblioteca b = CrearBibliotecaInicial();
        b.PrestarLibro("9781492056355", "Ana", out _);
        return !b.PrestarLibro("9781492056355", "Luis", out _);
    }, ref aprobadas);
    Probar("Persistencia JSON", () =>
    {
        string ruta = Path.Combine(Path.GetTempPath(), $"biblioteca-{Guid.NewGuid()}.json");
        try
        {
            Biblioteca b = CrearBibliotecaInicial();
            b.PrestarLibro("9781492056355", "Ana", out _);
            b.Guardar(ruta);
            Biblioteca copia = Biblioteca.Cargar(ruta);
            return copia.Catalogo.Count == 5 && copia.Prestamos["9781492056355"] == "Ana";
        }
        finally
        {
            if (File.Exists(ruta)) File.Delete(ruta);
        }
    }, ref aprobadas);

    Console.WriteLine($"\nResultado: {aprobadas}/5 pruebas aprobadas.");
}

static void Probar(string nombre, Func<bool> prueba, ref int aprobadas)
{
    bool resultado = prueba();
    Console.WriteLine($"[{(resultado ? "OK" : "ERROR")}] {nombre}");
    if (resultado) aprobadas++;
}

static void EjecutarBenchmark()
{
    Console.WriteLine("BENCHMARK DE BÚSQUEDA POR ISBN");
    Console.WriteLine($"{"Elementos",10} | {"Dictionary (µs)",16} | {"Recorrido (µs)",16} | Relación");
    Console.WriteLine(new string('-', 66));

    foreach (int cantidad in new[] { 100, 1_000, 10_000, 50_000 })
    {
        Dictionary<string, Libro> datos = new();
        for (int i = 0; i < cantidad; i++)
            datos[$"ISBN-{i:000000}"] = new Libro($"ISBN-{i:000000}", $"Libro {i}", "Autor", "Categoría", 2024);

        string objetivo = $"ISBN-{cantidad - 1:000000}";
        double directo = Medir(() =>
        {
            Libro? encontrado = null;
            for (int i = 0; i < 1_000; i++)
                encontrado = datos.GetValueOrDefault(objetivo);
            GC.KeepAlive(encontrado);
        }, 300, 1_000);
        double lineal = Medir(
            () => datos.Values.FirstOrDefault(libro => libro.Isbn == objetivo),
            300,
            1);
        Console.WriteLine($"{cantidad,10:N0} | {directo,16:F3} | {lineal,16:F3} | {lineal / directo,7:F1}x");
    }
}

static double Medir(Action accion, int repeticiones, int operacionesPorMuestra)
{
    long[] muestras = new long[repeticiones];
    for (int i = 0; i < repeticiones; i++)
    {
        long inicio = Stopwatch.GetTimestamp();
        accion();
        muestras[i] = Stopwatch.GetTimestamp() - inicio;
    }

    Array.Sort(muestras);
    return muestras[muestras.Length / 2] * 1_000_000.0 /
           Stopwatch.Frequency /
           operacionesPorMuestra;
}
