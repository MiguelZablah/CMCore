using System.Collections.Generic;
using CMCore.Interfaces;

namespace CMCore.DTO
{
    public class ClubDto : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public IList<TypeDto> Types { get; set; }

        public IList<RegionDto> Regions { get; set; }
    }
}
