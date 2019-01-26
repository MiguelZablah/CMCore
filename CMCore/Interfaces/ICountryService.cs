using System.Linq;
using System.Threading.Tasks;
using CMCore.DTO;
using CMCore.Models;

namespace CMCore.Interfaces
{
    public interface ICountryService
    {
        IQueryable<Country> FindAll(string name);
        IQueryable<Country> Exist(int id);
        string Validate(CountryDto countryDto);
        string CheckSameName(string name);
        bool Erase(Country countryInDb);
        Task<bool> SaveEf();
        CountryDto Edit(Country countryInDb, CountryDto countryDto);
        Country CreateNew(CountryDto countryDto);
        string AddRegionR(CountryDto countryDto, Region regionInDb);
    }
}
