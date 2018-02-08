namespace CMCore.Models.RelacionClass
{
    public class FileCompanie
    {
        public int FileId { get; set; }
        public File File { get; set; }

        public int CompanieId { get; set; }
        public Companie Companie { get; set; }
    }
}
