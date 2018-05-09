using CMCore.Models.RelacionClass;
using System.Collections.Generic;
using CMCore.Interfaces;

namespace CMCore.Models
{
    public class Region : IEntity
    {
        public Region()
        {
            ClubRegions = new List<ClubRegion>();
            Countries = new List<Countrie>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public IList<ClubRegion> ClubRegions { get; set; }

        public IList<Countrie> Countries { get; set; }
    }
}