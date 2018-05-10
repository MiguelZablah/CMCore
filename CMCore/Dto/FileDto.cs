﻿using System.Collections.Generic;
using CMCore.Interfaces;

namespace CMCore.DTO
{
    public class FileDto : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PathName { get; set; }

        public IList<TagDto> Tags { get; set; }

        public int ExtensionId { get; set; }
        public ExtensionDto Extension{ get; set; }

        public IList<CompanieDto> Companies { get; set; }

        public IList<ClubDto> Clubs { get; set; }
    }
}
