# Práctica experimental 03 - Biblioteca

Aplicación de consola en C# para registrar libros y gestionar préstamos mediante conjuntos, mapas y diccionarios.

## Estructuras utilizadas

- `Dictionary<string, Libro>`: catálogo indexado por ISBN.
- `Dictionary<string, HashSet<string>>`: mapas invertidos por autor y categoría.
- `HashSet<string>`: conjunto de ISBN disponibles.
- `Dictionary<string, string>`: relación entre ISBN prestado y lector.

## Ejecución

```powershell
dotnet run
dotnet run -- --demo
dotnet run -- --pruebas
dotnet run -- --benchmark
```

El modo normal abre el menú interactivo. Los otros modos generan evidencias reproducibles para el informe.
