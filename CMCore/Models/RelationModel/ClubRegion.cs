namespace CMCore.Models.RelationModel
{
    public class ClubRegion
    {
        public int ClubId { get; set; }
        public Club Club { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}
