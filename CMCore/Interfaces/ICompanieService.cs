using System.Linq;
using System.Threading.Tasks;
using CMCore.DTO;
using CMCore.Models;

namespace CMCore.Interfaces
{
    public interface ICompanieService
    {
        IQueryable<Companie> FindAll(string name);
        IQueryable<Companie> Exist(int id);
        IQueryable<Companie> ExistName(string name);
        string Validate(CompanieDto companieDto);
        string CheckSameName(CompanieDto companieDto);
        bool Erase(Companie companieInDb);
        Task<bool> SaveEf();
        CompanieDto Edit(Companie companieInDb, CompanieDto companieDto);
        Companie CreateNew(CompanieDto companieDto);
    }
}
