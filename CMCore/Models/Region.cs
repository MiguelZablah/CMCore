using System.Collections.Generic;
using CMCore.Interfaces;
using CMCore.Models.RelationModel;

namespace CMCore.Models
{
    public class Region : IEntity
    {
        public Region()
        {
            ClubRegions = new List<ClubRegion>();
            Countries = new List<Country>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public IList<ClubRegion> ClubRegions { get; set; }

        public IList<Country> Countries { get; set; }
    }
}