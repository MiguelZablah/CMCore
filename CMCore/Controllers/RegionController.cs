using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMCore.Controllers
{
    [Produces("application/json")]
    [Route("region/")]
    [EnableCors("AllowSpecificOrigin")]
    public class RegionController : Controller
    {
        private readonly IRegionService _regionService;

        public RegionController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        // GET region/
        [HttpGet]
        public IActionResult Get(string name = null)
        {
            var regions = _regionService.FindAll(name).ProjectTo<RegionDto>().ToList();
            if (!regions.Any())
                return BadRequest("No Regions");

            return Ok(regions);
        }

        // GET region/id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var regionInDb = _regionService.Exist(id).ProjectTo<RegionDto>().FirstOrDefault();
            if (regionInDb == null)
                return BadRequest("Region dosen't exist!");

            return Ok(regionInDb);
        }

        // PATCH region/id
        [HttpPatch("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] RegionDto regionDto)
        {
            if (regionDto == null)
                return BadRequest("You send a empty region");

            var regionInDb = _regionService.Exist(id).FirstOrDefault();
            if (regionInDb == null)
                return BadRequest("Region dosen't exist!");

            var errorMsg = _regionService.CheckSameName(regionDto);
            if (errorMsg != null)
                return BadRequest(errorMsg);

            if (regionDto.Name == null)
                regionDto.Name = regionInDb.Name;

            // Add countrie relacion
            var countryValMsg = _regionService.AddCountrieR(regionInDb, regionDto);
            if (countryValMsg != null)
                return BadRequest(countryValMsg);

            var newRegion = _regionService.Edit(regionInDb, regionDto);

            var saved = await _regionService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok(_regionService.Exist(newRegion.Id).ProjectTo<RegionDto>().FirstOrDefault());
        }

        // Delete region/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var regionInDb = _regionService.Exist(id).Include(r => r.Countries).FirstOrDefault();
            if (regionInDb == null)
                return BadRequest("Region dosen't exist!");

            var delete = _regionService.Erase(regionInDb);
            if (!delete)
                return BadRequest("Region not deleted!");

            var saved = await _regionService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok("Region Deleted: " + regionInDb.Name);
        }

        // POST region/
        [HttpPost]
        public async Task<IActionResult> New([FromBody] RegionDto regionDto)
        {
            var errorMsg = _regionService.Validate(regionDto);
            if (errorMsg != null)
                return BadRequest(errorMsg);

            var newRegion = _regionService.CreateNew(regionDto);

            // Add countrie relacion
            var countryValMsg = _regionService.AddCountrieR(newRegion, regionDto);
            if (countryValMsg != null)
                return BadRequest(countryValMsg);

            var saved = await _regionService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok(Mapper.Map<Region, RegionDto>(newRegion));
        }
    }
}