using Amazon;
using Amazon.S3;
using Amazon.S3.Model;


namespace ElProyecteGrandeBackend.Model;

public class AmazonS3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public AmazonS3Service(string accessKey, string secretKey, string bucketName, RegionEndpoint region)
    {
        _s3Client = new AmazonS3Client(accessKey, secretKey, region);
        _bucketName = bucketName;
    }

    public async Task<string> UploadObjectAsync(string key, IFormFile file)
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
}