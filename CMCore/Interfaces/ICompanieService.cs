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
        string Validate(CompanieDto companieDto);
        string CheckSameName(string name);
        bool Erase(Companie companieInDb);
        Task<bool> SaveEf();
        CompanieDto Edit(Companie companieInDb, CompanieDto companieDto);
        Companie CreateNew(CompanieDto companieDto);
        string AddFileR(CompanieDto companieDto, File fileInDb);
    }
}
