namespace CMCore.Models.RelationModel
{
    public class ClubType
    {
        public int ClubId { get; set; }
        public Club Club { get; set; }

        public int TypeId { get; set; }
        public Type Type { get; set; }
    }
}
