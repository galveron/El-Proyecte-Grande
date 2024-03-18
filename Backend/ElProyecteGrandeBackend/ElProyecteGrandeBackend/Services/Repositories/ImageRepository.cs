using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using ElProyecteGrandeBackend.Data;
using ElProyecteGrandeBackend.Services.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ElProyecteGrandeBackend.Model;

public class ImageRepository : IImageRepository
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly MarketPlaceContext _dbContext;
    private readonly IConfiguration _config;
    private readonly DbContextOptionsBuilder<MarketPlaceContext> _optionsBuilder;
    private readonly UserManager<User> _userManager;
    private readonly IProductRepository _productRepository;

    public ImageRepository(
        MarketPlaceContext dbContext,
        IConfiguration config,
        UserManager<User> userManager,
        IProductRepository productRepository)
    {
        _config =
            new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
        _optionsBuilder = new DbContextOptionsBuilder<MarketPlaceContext>();
        _optionsBuilder.UseSqlServer(_config["ConnectionString"]);
        var accessKey = config["AccessKey"] != null
            ? config["AccessKey"] : Environment.GetEnvironmentVariable("ACCESSKEY");
        var secretKey = config["SecretKey"] != null
            ? config["SecretKey"] : Environment.GetEnvironmentVariable("SECRETKEY");
        var bucketName = config["BucketName"] != null
            ? config["BucketName"] : Environment.GetEnvironmentVariable("BUCKETNAME");
        var region = RegionEndpoint.GetBySystemName("eu-north-1");
        _s3Client = new AmazonS3Client(accessKey, secretKey, region);
        _bucketName = bucketName;
        _dbContext = dbContext;
        _userManager = userManager;
        _productRepository = productRepository;
    }

    public async Task<string> UploadObjectAsyncToAWS(string key, IFormFile file)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = file.OpenReadStream(),
            ContentType = file.ContentType,
            CannedACL = S3CannedACL.PublicRead // Optional: Set the ACL for public read access
        };

        var response = await _s3Client.PutObjectAsync(request);

        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
        {
            return $"https://{_bucketName}.s3.amazonaws.com/{key}";
        }

        return null;
    }

    public async void UploadImageToDb(int productId, string userName, string imageUrl)
    {
        var product = _dbContext.Products
            .Include(user1 => user1.Images)
            .Include(user1 => user1.Seller)
            .FirstOrDefault(prod => prod.Id == productId);
        
        product.Images.Add(new Image{ ImageURL = imageUrl });
        _productRepository.UpdateProduct(product);

        _dbContext.Update(product);
        _dbContext.Products.Update(product);
    }
}