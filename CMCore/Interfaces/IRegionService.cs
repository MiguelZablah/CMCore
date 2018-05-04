using System.Collections.Generic;
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
        string CheckSameName(RegionDto regionDto);
        string Compare(Region regionInDb, RegionDto regionDto);
        string RegionCountrieRelation(Region regionInDb, RegionDto regionDto);
        Region CreateNew(RegionDto regionDto);
        bool Erase(Region regionInDb);
    }
}