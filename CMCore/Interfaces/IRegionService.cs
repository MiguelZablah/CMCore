using System.Collections.Generic;
using System.Threading.Tasks;
using CMCore.DTO;
using CMCore.Models;

namespace CMCore.Interfaces
{
    public interface IRegionService
    {
        List<RegionDto> FindAll(string name);
        Region Exist(int id);
        RegionDto Edit(Region regionInDb, RegionDto regionDto);
        string Validate(RegionDto regionDto);
        string Compare(Region regionInDb, RegionDto regionDto);
        bool Erase(Region regionInDb);
        Task<RegionDto> SaveNew(RegionDto regionDto);
        Region CreateNew(RegionDto regionDto);
        Task<RegionDto> Save(Region regionInDb);
    }
}