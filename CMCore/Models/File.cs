using CMCore.Models.RelacionClass;
using System.Collections.Generic;
using CMCore.Interfaces;

namespace CMCore.Models
{
    public class File : IEntity
    {
        public File()
        {
            FileTags = new List<FileTag>();
            FileCompanies = new List<FileCompanie>();
            FileClubs = new List<FileClub>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PathName { get; set; }

        public string AwsRegion { get; set; }

        public string ThumbUrl { get; set; }

        public IList<FileTag> FileTags { get; set; }

        public int ExtensionId { get; set; }
        public Extension Extension { get; set; }

        public IList<FileCompanie> FileCompanies { get; set; }

        public IList<FileClub> FileClubs { get; set; }
    }
}
