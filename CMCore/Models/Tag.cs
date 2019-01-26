using System.Collections.Generic;
using CMCore.Interfaces;
using CMCore.Models.RelationModel;

namespace CMCore.Models
{
    public class Tag : IEntity
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
