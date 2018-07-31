﻿using System.Linq;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;
using CMCore.Models.RelacionClass;
using Type = CMCore.Models.Type;

namespace CMCore.Services
{
    public class TypeService : GenericService<Type, TypeDto>, ITypeService
    {
        public TypeService(ContentManagerDbContext context) : base(context)
        {
        }

        public Type CreateNew(TypeDto typeDto)
        {
            var newType = new Type
            {
                Name = typeDto.Name
            };

            return AddEf(newType) ? newType : default(Type);
        }

        public string AddClubR(TypeDto typeDto, Club clubInDb)
        {
            var typenInDb = ExistName(typeDto.Name).FirstOrDefault();
            if (typenInDb == null)
            {
                var createdType = CreateNew(typeDto);
                if (createdType == null)
                    return "Type couden't be created!";

                var newType = createdType;
                var newClubType = new ClubType
                {
                    ClubId = clubInDb.Id,
                    TypeId = newType.Id
                };
                Context.ClubTypes.AddAsync(newClubType);

                return null;
            }

            if (string.IsNullOrEmpty(typeDto.Name))
                return "You send a null or empty Type!";

            if (!clubInDb.ClubTypes.Any(cr => cr.TypeId == typenInDb.Id))
            {
                var newClubType = new ClubType
                {
                    ClubId = clubInDb.Id,
                    TypeId = typenInDb.Id
                };
                Context.ClubTypes.AddAsync(newClubType);
            }

            return null;
        }

    }
}
