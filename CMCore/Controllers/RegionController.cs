using System.Threading.Tasks;
using AutoMapper;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;
using CMCore.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMCore.Controllers
{
    [Produces("application/json")]
    [Route("region/")]
    [EnableCors("AllowSpecificOrigin")]
    public class RegionController : Controller
    {
        private readonly IRegionService _regionService;
        private readonly IEfService _efService;

        public RegionController(IRegionService regionService, IEfService efService)
        {
            _regionService = regionService;
            _efService = efService;
        }

        // GET region/
        [HttpGet]
        public IActionResult Get(string name = null)
        {
            var regions = _regionService.FindAll(name);
            if (regions == null)
                return BadRequest("No Regions");

            return Ok(regions);
        }

        // GET region/id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var regionInDb = _regionService.Exist(id);
            if (regionInDb == null)
                return BadRequest("Region dosen't exist!");

            return Ok(Mapper.Map<Region, RegionDto>(regionInDb));
        }

        // PATCH region/id
        [HttpPatch("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] RegionDto regionDto)
        {
            if (regionDto == null)
                return BadRequest("You send a empty region");

            var regionInDb = _regionService.Exist(id);
            if (regionInDb == null)
                return BadRequest("Region dosen't exist!");

            var errorMsg = _regionService.CheckSameName(regionDto);
            if (errorMsg != null)
                return BadRequest(errorMsg);

            // Countrie check
            var countryValMsg = _regionService.RegionCountrieRelation(regionInDb, regionDto);
            if (countryValMsg != null)
                return BadRequest(countryValMsg);

            var newRegion = _regionService.Edit(regionInDb, regionDto);

            var saved = await _efService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok(Mapper.Map<Region, RegionDto>(_regionService.Exist(newRegion.Id)));
        }

        // Delete region/delete/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var regionInDb = _regionService.Exist(id);
            if (regionInDb == null)
                return BadRequest("Region dosen't exist!");

            var delete = _regionService.Erase(regionInDb);
            if (!delete)
                return BadRequest("Region not deleted!");

            var saved = await _efService.SaveEf();
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

            // Countrie check
            var countryValMsg = _regionService.RegionCountrieRelation(newRegion, regionDto);
            if (countryValMsg != null)
                return BadRequest(countryValMsg);

            var saved = await _efService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok(Mapper.Map<Region, RegionDto>(newRegion));
        }
    }
}