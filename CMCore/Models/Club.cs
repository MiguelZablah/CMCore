using System.Collections.Generic;
using CMCore.Interfaces;
using CMCore.Models.RelationModel;

namespace CMCore.Models
{
    public class Club : IEntity
    {
        public Club()
        {
            FileClubs = new List<FileClub>();
            ClubTypes = new List<ClubType>();
            ClubRegions = new List<ClubRegion>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public IList<FileClub> FileClubs { get; set; }

        public IList<ClubType> ClubTypes { get; set; }

        public IList<ClubRegion> ClubRegions { get; set; }
    }
}
