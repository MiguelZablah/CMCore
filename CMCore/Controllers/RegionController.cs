using System.Threading.Tasks;
using AutoMapper;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;
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
        private readonly ICountryService _countryService;

        public RegionController(IRegionService regionService, ICountryService countryService)
        {
            _regionService = regionService;
            _countryService = countryService;
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
        public IActionResult Edit(int id, [FromBody] RegionDto regionDto)
        {
            var regionInDb = _regionService.Exist(id);
            if (regionInDb == null)
                return BadRequest("Region dosen't exist!");

            var errorMsg = _regionService.Validate(regionDto);
            if (errorMsg != null)
                return BadRequest(errorMsg);

            var errMsg = _regionService.Compare(regionInDb, regionDto);
            if (errMsg != null)
                return BadRequest(errMsg);

            // Countrie check
            if (regionDto.Countries != null)
            {
                foreach (var country in regionDto.Countries)
                {
                    var countryErMsg = _countryService.EditSaveRegionR(country, regionInDb);
                    if (countryErMsg != null)
                        return BadRequest(countryErMsg);
                }
            }

            var regionSave = _regionService.Edit(regionInDb, regionDto);

            return Ok(regionSave);
        }

        // Delete region/delete/id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var regionInDb = _regionService.Exist(id);
            if (regionInDb == null)
                return BadRequest("Region dosen't exist!");

            var delete = _regionService.Erase(regionInDb);
            if (!delete)
                return BadRequest("Region not deleted!");

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
            if (regionDto.Countries != null)
            {
                foreach (var country in regionDto.Countries)
                {
                    var countryErMsg = _countryService.EditSaveRegionR(country, newRegion);
                    if (countryErMsg != null)
                        return BadRequest(countryErMsg);
                }
            }

            var regionNew = await _regionService.Save(newRegion);
            return Ok(regionNew);
        }
    }
}