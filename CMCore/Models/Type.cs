using CMCore.Models.RelacionClass;
using System.Collections.Generic;

namespace CMCore.Models
{
    public class Type
    {
        public Type()
        {
            ClubTypes = new List<ClubType>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public IList<ClubType> ClubTypes { get; set; }
    }
}