namespace CMCore.Models.RelationModel
{
    public class FileCompany
    {
        public int FileId { get; set; }
        public File File { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
