using System.Collections.Generic;
using CMCore.DTO;
using Type = CMCore.Models.Type;

namespace CMCore.Interfaces
{
    public interface ITypeService
    {
        List<TypeDto> FindAll(string name);
        Type Exist(int id);
        TypeDto Edit(Type typeInDb, TypeDto typeDto);
        string Validate(TypeDto typeDto);
        string CheckSameName(TypeDto typeDto);
        string Compare(Type typeInDb, TypeDto typeDto);
        Type CreateNew(TypeDto typeDto);
        bool Erase(Type typeInDb);
    }
}
