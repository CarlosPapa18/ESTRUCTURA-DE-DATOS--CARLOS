namespace PracticoExperimental03;

public sealed record Libro(
    string Isbn,
    string Titulo,
    string Autor,
    string Categoria,
    int Anio)
{
    public override string ToString() =>
        $"{Isbn} | {Titulo} | {Autor} | {Categoria} | {Anio}";
}
