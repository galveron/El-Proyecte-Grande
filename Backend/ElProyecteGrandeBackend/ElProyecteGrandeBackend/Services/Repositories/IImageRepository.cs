namespace ElProyecteGrandeBackend.Model;

public interface IImageRepository
{
    Task<string> UploadObjectAsyncToAWS(string key, IFormFile file);
    void UploadImageToDb(int productId, string userId, string imageUrl);
}