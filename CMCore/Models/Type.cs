﻿using System.Collections.Generic;
using CMCore.Interfaces;
using CMCore.Models.RelationModel;

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