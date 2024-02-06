namespace ElProyecteGrandeBackend.Model;

public class Company
{
    public string Name { get; init; }
    public string Identifier { get; init; } //pl.: Cégjegyzékszám Magyarországon
    public bool Verified { get; init; }
}