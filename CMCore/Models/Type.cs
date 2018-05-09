using CMCore.Models.RelacionClass;
using System.Collections.Generic;
using CMCore.Interfaces;

namespace CMCore.Models
{
    public class Type : IEntity
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