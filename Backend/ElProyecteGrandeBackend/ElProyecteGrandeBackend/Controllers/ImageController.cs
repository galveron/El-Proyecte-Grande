using Amazon;
using ElProyecteGrandeBackend.Model;
using ElProyecteGrandeBackend.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElProyecteGrandeBackend.Controllers;
[ApiController]
[Route("Image")]
public class ImageController : ControllerBase
{
        private readonly IImageRepository _imageRepository;
        private readonly IProductRepository _productRepository;

        public ImageController(IConfiguration config, IImageRepository imageRepository, IProductRepository productRepository)
        {
            var accessKey = config["AccessKey"] != null
                ? config["AccessKey"] : Environment.GetEnvironmentVariable("ACCESSKEY");
            var secretKey = config["SecretKey"] != null
                ? config["SecretKey"] : Environment.GetEnvironmentVariable("SECRETKEY");
            var bucketName = config["BucketName"] != null
                ? config["BucketName"] : Environment.GetEnvironmentVariable("BUCKETNAME");
            var region = RegionEndpoint.GetBySystemName("eu-north-1");

            _imageRepository = imageRepository;
            _productRepository = productRepository;
        }

        [HttpPost("upload"), Authorize(Roles="Admin, Company")]
        public async Task<IActionResult> UploadImage(IFormFile file, int productId)
        {
            var product = _productRepository.GetProduct(productId);

            if (product == null)
            {
                return NotFound("Product is not found");
            }
            
            var key = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var imageUrl = await _imageRepository.UploadObjectAsyncToAWS(key, file);
            _imageRepository.UploadImageToDb(productId, User.Identity.Name, imageUrl);

            return Ok(new { ImageUrl = imageUrl });
        }
}