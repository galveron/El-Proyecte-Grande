using Amazon;
using ElProyecteGrandeBackend.Model;
using Microsoft.AspNetCore.Mvc;

namespace ElProyecteGrandeBackend.Controllers;
[ApiController]
[Route("Image")]
public class ImageController : ControllerBase
{
        private readonly AmazonS3Service _s3Service;

        public ImageController(IConfiguration configuration)
        {
            var accessKey = "AKIA6GBMCS7P6VWY4KEK";
            var secretKey = "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";
            var bucketName = "elproyectegrande";
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