using System.Linq;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;

namespace CMCore.Services
{
    public class CountryService : GenericService<Country, CountryDto>, ICountryService
    {
        public CountryService(ContentManagerDbContext context) : base(context)
        {
        }

        public Country CreateNew(CountryDto countryDto)
        {
            var newCountry = new Country
            {
                Name = countryDto.Name
            };

            return AddEf(newCountry) ? newCountry : default(Country);
        }

        public string AddRegionR(CountryDto countryDto, Region regionInDb)
        {
            var countryInDb = ExistName(countryDto.Name).FirstOrDefault();
            if (countryInDb == null)
            {
                var createCountry = new Country
                {
                    Name = countryDto.Name,
                    RegionId = regionInDb.Id
                };
                AddEf(createCountry);
                return null;
            }

            if (string.IsNullOrEmpty(countryDto.Name))
                return "You send a null or empty country!";

            var regionHasCountry = regionInDb.Countries.Any(cr => cr.Id == countryInDb.Id);
            if (!regionHasCountry)
            {
                Context.Entry(countryInDb).Property(c => c.RegionId).CurrentValue = regionInDb.Id;
            }

            return null;

        }

    }
}
