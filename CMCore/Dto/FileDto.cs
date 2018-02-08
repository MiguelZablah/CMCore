using System.Collections.Generic;

namespace CMCore.DTO
{
    public class FileDto
    {
        public FileDto()
        {
            Tags = new List<TagDto>();
            Companies = new List<CompanieDto>();
            Clubs = new List<ClubDto>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IList<TagDto> Tags { get; set; }

        public ExtensionDto Extension { get; set; }

        public IList<CompanieDto> Companies { get; set; }

        public IList<ClubDto> Clubs { get; set; }
    }
}
