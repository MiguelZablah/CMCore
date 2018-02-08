using System.Collections.Generic;

namespace CMCore.DTO
{
    public class ClubDto
    {
        public ClubDto()
        {
            Types = new List<TypeDto>();
            Regions = new List<RegionDto>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public IList<TypeDto> Types { get; set; }

        public IList<RegionDto> Regions { get; set; }
    }
}
