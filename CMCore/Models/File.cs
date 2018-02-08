using CMCore.Models.RelacionClass;
using System.Collections.Generic;

namespace CMCore.Models
{
    public class File
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

        public IList<FileTag> FileTags { get; set; }

        public int ExtensionId { get; set; }
        public Extension Extension { get; set; }

        public IList<FileCompanie> FileCompanies { get; set; }

        public IList<FileClub> FileClubs { get; set; }
    }
}
