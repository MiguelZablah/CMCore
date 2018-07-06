using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
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

        public string UploadFile(IFormFile file, string fileName)
        {
            if(!CheckRequiredFields(fileName))
                return "S3 Missing Data!";

            UploadS3Async(file, fileName).Wait();
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

                Console.WriteLine("Upload completed");

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
    }
}
