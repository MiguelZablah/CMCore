using System.Linq;
using System.Threading.Tasks;
using CMCore.DTO;
using CMCore.Models;

namespace CMCore.Interfaces
{
    public interface IRegionService
    {
        IQueryable<Region> FindAll(string name);
        IQueryable<Region> Exist(int id);
        string Validate(RegionDto regionDto);
        string CheckSameName(string name);
        bool Erase(Region regionInDb);
        Task<bool> SaveEf();
        RegionDto Edit(Region regionInDb, RegionDto regionDto);
        Region CreateNew(RegionDto regionDto);
        string AddCountrieR(Region regionInDb, RegionDto regionDto);
        string AddClubR(RegionDto regionDto, Club clubInDb);
        bool ClearRelations(Region regionInDb);
    }
}