using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using CMCore.Helpers;
using CMCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CMCore.Services
{
    public class AwsS3Service : IAwsS3Service
    {
        private readonly AwsSettings _awSettings;
        private readonly IAmazonS3 _s3Client;

        public AwsS3Service(IOptions<AwsSettings> awsSettings)
        {
            _awSettings = awsSettings.Value;
            _s3Client = new AmazonS3Client(RegionEndpoint.GetBySystemName(_awSettings.Region));
        }

        public Tuple<string, string> UploadFile(IFormFile file, string fileName, string extension)
        {
            if(!CheckRequiredFields(fileName + extension))
                return null;

            UploadS3Async(file, fileName + extension).Wait();

            var tuple = new Tuple<string, string>(_awSettings.Region, $"https://s3.amazonaws.com/{_awSettings.BucketName}/{_awSettings.S3FolderThumbs}{fileName}.jpg");
            return tuple;
        }

        public string DowloadUrl(string fileName, string fileRegion)
        {
            if (!CheckRequiredFields(fileName))
                return null;

            var newS3Client = RegionEndpoint.GetBySystemName(_awSettings.Region);
            if (!string.IsNullOrWhiteSpace(fileRegion))
                newS3Client = RegionEndpoint.GetBySystemName(fileRegion);

            var urlString = GeneratePreSignedUrl(fileName, newS3Client);
            if(string.IsNullOrWhiteSpace(urlString))
                return null;

            return urlString;
        }

        public string DeleteFile(string fileName, string fileRegion, bool deleteThumb)
        {
            if (!CheckRequiredFields(fileName))
                return "S3 Missing Data!";

            var newS3Client = RegionEndpoint.GetBySystemName(_awSettings.Region);
            if (!string.IsNullOrWhiteSpace(fileRegion))
                newS3Client = RegionEndpoint.GetBySystemName(fileRegion);

            DeleteS3Async(fileName, newS3Client, false).Wait();
            if(deleteThumb)
                DeleteS3Async(fileName, newS3Client, true).Wait();
            return null;
        }

        private bool CheckRequiredFields(string keyName)
        {
            if (string.IsNullOrEmpty(_awSettings.BucketName))
            {
                Console.WriteLine("The variable bucketName is not set.");
                return false;
            }
            if (string.IsNullOrEmpty(keyName))
            {
                Console.WriteLine("The variable keyName is not set.");
                return false;
            }

            return true;
        }

        private async Task UploadS3Async(IFormFile file, string keyName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_s3Client);
                var fileStream = file.OpenReadStream();

                await fileTransferUtility.UploadAsync(fileStream, _awSettings.BucketName, _awSettings.S3Folder + keyName);
                //Console.WriteLine("Upload completed");
            }
            catch (AmazonS3Exception e)
            {
                var err = $"Error encountered on server. Message:'{e.Message}' when writing an object";
                Console.WriteLine(err);
            }
            catch (Exception e)
            {
                var err = $"Unknown encountered on server. Message:'{e.Message}' when writing an object";
                Console.WriteLine(err);
            }
        }

        private async Task DeleteS3Async(string keyName, RegionEndpoint region, bool deleteThumb)
        {
            var client = new AmazonS3Client(region);
            var keyString = deleteThumb
                ? _awSettings.S3FolderThumbs + keyName.Remove(keyName.LastIndexOf(".", StringComparison.Ordinal) + 1) + "jpg"
                : _awSettings.S3Folder + keyName;
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _awSettings.BucketName,
                    Key = keyString
                };

                //Console.WriteLine("Deleting an object");
                await client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine($"Error encountered on server. Message:'{e.Message}' when writing an object");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unknown encountered on server. Message:'{e.Message}' when writing an object");
            }
        }

        private string GeneratePreSignedUrl(string keyName, RegionEndpoint region)
        {
            var urlString = "";
            var client = new AmazonS3Client(region);
            try
            {
                var request1 = new GetPreSignedUrlRequest
                {
                    BucketName = _awSettings.BucketName,
                    Key = _awSettings.S3Folder + keyName,
                    Expires = DateTime.Now.AddMinutes(_awSettings.TimePreSigUrl)
                };
                urlString = client.GetPreSignedURL(request1);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine($"Error encountered on server. Message:'{e.Message}' when writing an object");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unknown encountered on server. Message:'{e.Message}' when writing an object");
            }
            return urlString;
        }
    }
}
