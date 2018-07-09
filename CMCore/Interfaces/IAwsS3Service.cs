using Microsoft.AspNetCore.Http;

namespace CMCore.Interfaces
{
    public interface IAwsS3Service
    {
        string UploadFile(IFormFile file, string fileName);
        string DowloadUrl(string fileName);
    }
}
