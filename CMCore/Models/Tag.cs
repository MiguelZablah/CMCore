using CMCore.Models.RelacionClass;
using System.Collections.Generic;

namespace CMCore.Models
{
    public class Tag
    {
        public Tag()
        {
            FileTags = new List<FileTag>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public IList<FileTag> FileTags { get; set; }
    }
}
