using System.Collections.Generic;
using System.Threading.Tasks;
using CMCore.DTO;
using CMCore.Models;

namespace CMCore.Interfaces
{
    public interface ICountryService
    {
        List<CountrieDto> FindAll(string name);
        Countrie Exist(int id);
        CountrieDto Edit(Countrie countrieInDb, CountrieDto countrieDto);
        string Validate(CountrieDto countrieDto);
        string Compare(Countrie countrieInDb, CountrieDto countrieDto);
        bool Erase(Countrie countrieInDb);
        Task<CountrieDto> SaveNew(CountrieDto countrieDto);
        Countrie CreateNew(CountrieDto countrieDto);
        string EditSaveRegionR(CountrieDto countrieDto, Region regionInDb);
    }
}
