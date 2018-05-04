using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;
using Microsoft.EntityFrameworkCore;

namespace CMCore.Services
{
    public class RegionService : IRegionService
    {
        private readonly ContentManagerDbContext _context;
        private readonly ICountryService _countryService;

        public RegionService(ContentManagerDbContext context, ICountryService countryService)
        {
            _context = context;
            _countryService = countryService;
        }

        public List<RegionDto> FindAll(string name)
        {
            var regionsQuery = _context.Regions.ProjectTo<RegionDto>();

            if (!string.IsNullOrWhiteSpace(name))
                regionsQuery = regionsQuery.Where(f => f.Name.ToLower().Contains(name));

            var regions = regionsQuery.ToList();

            if (regions.Count <= 0)
                return null;

            return regions;
        }

        public Region Exist(int id)
        {
            try
            {
                var regionInDb = _context.Regions.Include("Countries").SingleOrDefault(c => c.Id == id);
                return regionInDb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public Region ExistName(string name)
        {
            try
            {
                var regionInDb = _context.Regions.Include("Countries").SingleOrDefault(c => c.Name.ToLower() == name.ToLower());
                return regionInDb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public string Validate(RegionDto regionDto)
        {

            var checkName = CheckSameName(regionDto);
            if (checkName != null)
            {
                return checkName;
            }

            return string.IsNullOrEmpty(regionDto.Name) ? "You send a null or empty string!" : null;
        }

        public string CheckSameName(RegionDto regionDto)
        {
            if (!string.IsNullOrEmpty(regionDto.Name))
            {
                if (_context.Regions.Any(t => t.Name.ToLower() == regionDto.Name.ToLower()))
                    return "A Region with that name already exist!";
            }

            return null;
        }

        public string Compare(Region regionInDb, RegionDto regionDto)
        {
            if (!string.IsNullOrEmpty(regionDto.Name))
            {
                if (regionInDb.Name.ToLower() == regionDto.Name.ToLower())
                    return "Same name, not changes made";

            }

            return null;
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
            _context.Regions.Add(newRegion);
            return newRegion;
        }

        public string RegionCountrieRelation(Region regionInDb, RegionDto regionDto)
        {
            if (regionDto.Countries != null)
            {
                foreach (var country in regionDto.Countries)
                {
                    var countryErMsg = _countryService.EditSaveRegionR(country, regionInDb);
                    if (countryErMsg != null)
                        return countryErMsg;
                }
                return null;
            }

            return null;
        }

        public bool Erase(Region regionInDb)
        {
            try
            {
                _context.Regions.Remove(regionInDb);
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
