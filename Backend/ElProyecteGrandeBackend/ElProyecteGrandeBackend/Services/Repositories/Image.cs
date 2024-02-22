namespace ElProyecteGrandeBackend.Model;

public class Image
{
    public int Id { get; init; }
    public Product Product { get; init; }
    public string ImageName { get; init; }
    public byte[] ImageData { get; init; }
}