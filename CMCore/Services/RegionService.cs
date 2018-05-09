using AutoMapper;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;

namespace CMCore.Services
{
    public class RegionService : GenericService<Region, RegionDto>, IRegionService
    {
        private readonly ICountryService _countryService;

        public RegionService(ContentManagerDbContext context, ICountryService countryService) : base(context)
        {
            _countryService = countryService;
        }

        public RegionDto Edit(Region regionInDb, RegionDto regionDto)
        {
            if (Compare(regionInDb, regionDto) != null)
                return Mapper.Map<Region, RegionDto>(regionInDb);

            if(string.IsNullOrEmpty(regionDto.Name))
                return Mapper.Map<Region, RegionDto>(regionInDb);

            regionInDb.Name = regionDto.Name;
            
            return Mapper.Map<Region, RegionDto>(regionInDb);
        }

        public Region CreateNew(RegionDto regionDto)
        {
            var newRegion = new Region
            {
                Name = regionDto.Name
            };

            return AddEf(newRegion) ? newRegion : default(Region);
        }

        public string AddCountrieR(Region regionInDb, RegionDto regionDto)
        {
            if (regionDto.Countries != null)
            {
                foreach (var countrie in regionDto.Countries)
                {
                    var countrieErMsg = _countryService.AddRegionR(countrie, regionInDb);
                    if (countrieErMsg != null)
                        return countrieErMsg;
                }
                return null;
            }

            return null;
        }

    }
}
