using Microsoft.EntityFrameworkCore;

namespace ElProyecteGrandeBackend.Model;

[Owned]
public class Company
{
    public string Name { get; set; }
    public string Identifier { get; set; } //pl.: Cégjegyzékszám Magyarországon
    public bool Verified { get; set; }
}