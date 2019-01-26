using System;
using Microsoft.AspNetCore.Http;

namespace CMCore.Interfaces
{
    public interface IAwsS3Service
    {
        Tuple<string, string> UploadFile(IFormFile file, string fileName, string extension);
        string DownloadUrl(string fileName, string fileRegion = null);
        string DeleteFile(string fileName, string fileRegion, bool deleteThumb = false);
    }
}
