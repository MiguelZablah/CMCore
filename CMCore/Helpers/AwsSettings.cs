namespace CMCore.Helpers
{
    public class AwsSettings
    {
        public string Region { get; set; }
        public string BucketName { get; set; }
        public string S3Folder { get; set; }
        public string S3FolderThumbs { get; set; }
        public int TimePreSigUrl { get; set; }
    }
}
