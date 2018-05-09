using System.Collections.Generic;
using CMCore.Interfaces;

namespace CMCore.DTO
{
    public class RegionDto : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<CountrieDto> Countries { get; set; }
    }
}