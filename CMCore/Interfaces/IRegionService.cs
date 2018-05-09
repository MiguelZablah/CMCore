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
        string CheckSameName(RegionDto regionDto);
        string Compare(Region regionInDb, RegionDto regionDto);
        bool Erase(Region regionInDb);
        Task<bool> SaveEf();
        string AddCountrieR(Region regionInDb, RegionDto regionDto);
        RegionDto Edit(Region regionInDb, RegionDto regionDto);
        Region CreateNew(RegionDto regionDto);
    }
}