using System.Linq;
using System.Threading.Tasks;
using CMCore.DTO;
using CMCore.Models;

namespace CMCore.Interfaces
{
    public interface IClubService
    {
        IQueryable<Club> FindAll(string name);
        IQueryable<Club> Exist(int id);
        string Validate(ClubDto clubDto);
        string CheckSameName(string name);
        bool Erase(Club clubInDb);
        Task<bool> SaveEf();
        ClubDto Edit(Club clubInDb, ClubDto clubDto);
        Club CreateNew(ClubDto clubDto);
        string AddRegionCountriR(Club clubInDb, ClubDto clubDto);
        string AddTypeR(Club clubInDb, ClubDto clubDto);
        string AddFileR(ClubDto clubDto, File fileInDb);
        bool ClearRelations(Club clubInDb);
    }
}
