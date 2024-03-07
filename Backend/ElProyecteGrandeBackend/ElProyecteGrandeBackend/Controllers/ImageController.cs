using Amazon;
using ElProyecteGrandeBackend.Model;
using Microsoft.AspNetCore.Mvc;

namespace ElProyecteGrandeBackend.Controllers;
[ApiController]
[Route("Image")]
public class ImageController : ControllerBase
{
        private readonly AmazonS3Service _s3Service;

        public ImageController(IConfiguration config)
        {
            var accessKey = config["AccessKey"] != null
                ? config["AccessKey"] : Environment.GetEnvironmentVariable("ACCESSKEY");
            var secretKey = config["SecretKey"] != null
                ? config["SecretKey"] : Environment.GetEnvironmentVariable("SECRETKEY");
            var bucketName = config["BucketName"] != null
                ? config["BucketName"] : Environment.GetEnvironmentVariable("BUCKETNAME");
            var region = RegionEndpoint.GetBySystemName("eu-north-1");

            _s3Service = new AmazonS3Service(accessKey, secretKey, bucketName, region);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var key = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var imageUrl = await _s3Service.UploadObjectAsync(key, file);

            Console.WriteLine(imageUrl);
            // Save the imageUrl in your database or perform other operations

            return Ok(new { ImageUrl = imageUrl });
        }
}