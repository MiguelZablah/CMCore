using CMCore.Interfaces;

namespace CMCore.DTO
{
    public class TagDto : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
