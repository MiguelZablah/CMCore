using CMCore.Interfaces;

namespace CMCore.Models
{
    public class Countrie : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? RegionId { get; set; }
        public Region Region { get; set; }
    }
}