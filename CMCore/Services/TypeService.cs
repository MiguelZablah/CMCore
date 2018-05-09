using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using Type = CMCore.Models.Type;

namespace CMCore.Services
{
    public class TypeService : GenericService<Type, TypeDto>, ITypeService
    {
        public TypeService(ContentManagerDbContext context) : base(context)
        {
        }

        public TypeDto Edit(Type typeInDb, TypeDto typeDto)
        {

            if (Compare(typeInDb, typeDto) != null)
                return Mapper.Map<Type, TypeDto>(typeInDb);

            if (string.IsNullOrEmpty(typeDto.Name))
                return Mapper.Map<Type, TypeDto>(typeInDb);

            typeInDb.Name = typeDto.Name;

            return Mapper.Map<Type, TypeDto>(typeInDb);
        }

        public Type CreateNew(TypeDto typeDto)
        {
            var newType = new Type
            {
                Name = typeDto.Name
            };

            return AddEf(newType) ? newType : default(Type);
        }

    }
}
