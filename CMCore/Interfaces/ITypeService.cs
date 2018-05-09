using System.Linq;
using System.Threading.Tasks;
using CMCore.DTO;
using CMCore.Models;
using Type = CMCore.Models.Type;

namespace CMCore.Interfaces
{
    public interface ITypeService
    {
        IQueryable<Type> FindAll(string name);
        IQueryable<Type> Exist(int id);
        IQueryable<Type> ExistName(string name);
        string Validate(TypeDto typeDto);
        string CheckSameName(TypeDto typeDto);
        bool Erase(Type typeInDb);
        Task<bool> SaveEf();
        TypeDto Edit(Type typeInDb, TypeDto typeDto);
        Type CreateNew(TypeDto typeDto);
        string AddClubR(TypeDto typeDto, Club clubInDb);
    }
}
