using System.Collections.Generic;
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
        string CheckSameName(CountrieDto countriDto);
        string Compare(Countrie countrieInDb, CountrieDto countrieDto);
        string EditSaveRegionR(CountrieDto countrieDto, Region regionInDb);
        Countrie CreateNew(CountrieDto countrieDto);
        bool Erase(Countrie countrieInDb);
    }
}
