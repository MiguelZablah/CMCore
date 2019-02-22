using System;
using System.Linq;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;
using CMCore.Models.RelationModel;

namespace CMCore.Services
{
    public class RegionService : GenericService<Region, RegionDto>, IRegionService
    {
        private readonly ICountryService _countryService;

        public RegionService(ContentManagerDbContext context, ICountryService countryService) : base(context)
        {
            _countryService = countryService;
        }

        public Region CreateNew(RegionDto regionDto)
        {
            var newRegion = new Region
            {
                Name = regionDto.Name
            };

            return AddEf(newRegion) ? newRegion : default(Region);
        }

        public string AddCountryR(Region regionInDb, RegionDto regionDto)
        {
            if (regionDto.Countries != null)
            {
                foreach (var country in regionDto.Countries)
                {
                    var countryErMsg = _countryService.AddRegionR(country, regionInDb);
                    if (!string.IsNullOrWhiteSpace(countryErMsg))
                        return countryErMsg;
                }
                return null;
            }

            return null;
        }

        public string AddClubR(RegionDto regionDto, Club clubInDb)
        {
            var regionInDb = ExistName(regionDto.Name, true).FirstOrDefault();
            if (regionInDb == null)
            {
                var createdRegion = CreateNew(regionDto);
                if (createdRegion == null)
                    return "Region couldn't be created!";

                var newRegion = createdRegion;
                var newClubRegionRelation = new ClubRegion
                {
                    ClubId = clubInDb.Id,
                    RegionId = newRegion.Id
                };
                Context.ClubRegions.AddAsync(newClubRegionRelation);

                // Add country relation
                var countryValMsg = AddCountryR(newRegion, regionDto);
                if (!string.IsNullOrWhiteSpace(countryValMsg))
                    return countryValMsg;

                return null;
            }

            if (string.IsNullOrEmpty(regionDto.Name))
                return "You send a null or empty region!";

            var regionHasClub = clubInDb.ClubRegions.Any(cr => cr.RegionId == regionInDb.Id);
            if (!regionHasClub)
            {
                var newClubRegionRelation = new ClubRegion
                {
                    ClubId = clubInDb.Id,
                    RegionId = regionInDb.Id
                };
                Context.ClubRegions.AddAsync(newClubRegionRelation);
            }

            // Add country relation
            var countryValMessage = AddCountryR(regionInDb, regionDto);
            return !string.IsNullOrWhiteSpace(countryValMessage)
                ? countryValMessage
                : null;
        }

        public bool ClearRelations(Region regionInDb)
        {
            try
            {
                regionInDb.Countries.Clear();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
