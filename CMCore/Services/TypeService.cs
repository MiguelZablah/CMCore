using System.Linq;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;
using CMCore.Models.RelationModel;
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
            var typeInDb = ExistName(typeDto.Name).FirstOrDefault();
            if (typeInDb == null)
            {
                var createdType = CreateNew(typeDto);
                if (createdType == null)
                    return "Type couldn't be created!";

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

            if (!clubInDb.ClubTypes.Any(cr => cr.TypeId == typeInDb.Id))
            {
                var newClubType = new ClubType
                {
                    ClubId = clubInDb.Id,
                    TypeId = typeInDb.Id
                };
                Context.ClubTypes.AddAsync(newClubType);
            }

            return null;
        }

    }
}
