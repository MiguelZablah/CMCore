using System.Collections.Generic;
using CMCore.DTO;
using CMCore.Models;

namespace CMCore.Interfaces
{
    public interface ICompanieService
    {
        List<CompanieDto> FindAll(string name);
        Companie Exist(int id);
        CompanieDto Edit(Companie companieInDb, CompanieDto companieDto);
        string Validate(CompanieDto companieDto);
        string CheckSameName(CompanieDto companieDto);
        string Compare(Companie companieInDb, CompanieDto companieDto);
        Companie CreateNew(CompanieDto companieDto);
        bool Erase(Companie companieInDb);
    }
}
