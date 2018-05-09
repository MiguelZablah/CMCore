using CMCore.Interfaces;

namespace CMCore.DTO
{
    public class CompanieDto : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
