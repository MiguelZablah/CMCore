namespace CMCore.Models.RelacionClass
{
    public class FileClub
    {
        public int FileId { get; set; }
        public File File { get; set; }

        public int ClubId { get; set; }
        public Club Club { get; set; }
    }
}
