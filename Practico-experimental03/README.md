# Práctica experimental 03 - Registro de libros

Aplicación de consola en C# para registrar, organizar, buscar y consultar libros de una biblioteca mediante conjuntos, mapas y diccionarios.

## Archivos principales

- `Libro.cs`: define los datos de cada libro: ISBN, título, autor, categoría y año.
- `Biblioteca.cs`: contiene las estructuras de datos y las operaciones del sistema.
- `Program.cs`: presenta el menú y permite registrar, buscar y mostrar reportes.

## Estructuras solicitadas por el profesor

- `Dictionary<string, Libro>`: diccionario principal; relaciona cada ISBN único con un libro.
- `Dictionary<string, HashSet<string>>`: mapas por autor y por categoría; relacionan una clave con varios ISBN.
- `HashSet<string>`: conjunto de ISBN disponibles; evita duplicados y permite comprobar pertenencia.
- `Dictionary<string, string>`: relaciona cada libro prestado con el lector responsable.

## Funciones que se deben demostrar

1. Registrar un libro y rechazar un ISBN repetido.
2. Buscar libros por ISBN, autor y categoría.
3. Mostrar un reporte claro del catálogo y sus totales.
4. Explicar la función de cada conjunto, mapa y diccionario.
5. Comparar el tiempo de búsqueda del diccionario con un recorrido lineal.

## Cómo ejecutar

```powershell
dotnet run
dotnet run -- --demo
dotnet run -- --pruebas
dotnet run -- --benchmark
```

El menú permite realizar el flujo manual. Los otros modos reproducen las evidencias, comprueban el funcionamiento y muestran la medición de tiempo incluida en el informe.
