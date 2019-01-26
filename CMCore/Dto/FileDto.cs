using System.Collections.Generic;
using CMCore.Interfaces;

namespace CMCore.DTO
{
    public class FileDto : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PathName { get; set; }

        public string ThumbUrl { get; set; }

        public string Extension { get; set; }

        public IList<TagDto> Tags { get; set; }

        public IList<CompanyDto> Companies { get; set; }

        public IList<ClubDto> Clubs { get; set; }
    }
}
