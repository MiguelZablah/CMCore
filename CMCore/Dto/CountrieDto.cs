using CMCore.Interfaces;

namespace CMCore.DTO
{
    public class CountrieDto : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}