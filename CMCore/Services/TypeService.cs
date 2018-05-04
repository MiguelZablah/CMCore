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
    public class TypeService : ITypeService
    {
        private readonly ContentManagerDbContext _context;

        public TypeService(ContentManagerDbContext context)
        {
            _context = context;
        }

        public List<TypeDto> FindAll(string name)
        {
            var typesQuery = _context.Types.ProjectTo<TypeDto>();

            if (!string.IsNullOrWhiteSpace(name))
                typesQuery = typesQuery.Where(f => f.Name.ToLower().Contains(name));

            var types = typesQuery.ToList();

            if (types.Count <= 0)
                return null;

            return types;
        }

        public Type Exist(int id)
        {
            try
            {
                var typeInDb = _context.Types.SingleOrDefault(c => c.Id == id);
                return typeInDb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public Type ExistName(string name)
        {
            try
            {
                var typeInDb = _context.Types.SingleOrDefault(t => t.Name.ToLower() == name.ToLower());
                return typeInDb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public string Validate(TypeDto typeDto)
        {

            var checkName = CheckSameName(typeDto);
            if (checkName != null)
                return checkName;

            return string.IsNullOrEmpty(typeDto.Name) ? "You send a null or empty string!" : null;
        }

        public string CheckSameName(TypeDto typeDto)
        {
            if (!string.IsNullOrEmpty(typeDto.Name))
            {
                if (_context.Types.Any(t => t.Name.ToLower() == typeDto.Name.ToLower()))
                    return "A Type with that name already exist!";
            }

            return null;
        }

        public string Compare(Type typeInDb, TypeDto typeDto)
        {
            if (!string.IsNullOrEmpty(typeDto.Name))
            {
                if (typeInDb.Name.ToLower() == typeDto.Name.ToLower())
                    return "Same name, not changes made";

            }

            return null;
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
            _context.Types.Add(newType);
            return newType;
        }

        public bool Erase(Type typeInDb)
        {
            try
            {
                _context.Types.Remove(typeInDb);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
