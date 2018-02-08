using System.Collections.Generic;

namespace CMCore.Models
{
    public class Extension
    {
        public Extension()
        {
            Files = new List<File>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public IList<File> Files { get; set; }
    }
}
