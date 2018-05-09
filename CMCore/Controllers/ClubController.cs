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
    [Route("club/")]
    [EnableCors("AllowSpecificOrigin")]
    public class ClubController : Controller
    {

        private readonly IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }

        // GET club/
        [HttpGet]
        public IActionResult Get(string name = null)
        {
            var club = _clubService.FindAll(name).ProjectTo<ClubDto>().ToList();
            if (!club.Any())
                return BadRequest("No Clubs");

            return Ok(club);
        }

        // GET club/id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var clubInDb = _clubService.Exist(id).ProjectTo<ClubDto>().FirstOrDefault();
            if (clubInDb == null)
                return BadRequest("Club dosen't exist!");

            return Ok(clubInDb);
        }

        // PATCH club/id
        [HttpPatch("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] ClubDto clubDto)
        {
            if (clubDto == null)
                return BadRequest("You send an invalid club");

            var clubInDb = _clubService.Exist(id).FirstOrDefault();
            if (clubInDb == null)
                return BadRequest("Club dosen't exist!");

            var errorMsg = _clubService.CheckSameName(clubDto);
            if (errorMsg != null)
                return BadRequest(errorMsg);

            clubDto = _clubService.Edit(clubInDb, clubDto);

            // Validate and Add Types
            var countryValMsg = _clubService.AddTypeR(clubInDb, clubDto);
            if (countryValMsg != null)
                return BadRequest(countryValMsg);

            // Validate and Add Region and/or club
            var countryValMsgg = _clubService.AddRegionCountriR(clubInDb, clubDto);
            if (countryValMsgg != null)
                return BadRequest(countryValMsgg);

            var newRegion = _clubService.Edit(clubInDb, clubDto);

            var saved = await _clubService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok(_clubService.Exist(newRegion.Id).ProjectTo<ClubDto>().FirstOrDefault());
        }

        //    // Delete club/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var clubInDb = _clubService.Exist(id).Include(r => r.ClubRegions).Include(r => r.ClubTypes).FirstOrDefault();
            if (clubInDb == null)
                return BadRequest("Club dosen't exist!");

            var delete = _clubService.Erase(clubInDb);
            if (!delete)
                return BadRequest("Club not deleted!");

            var saved = await _clubService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok("Region Deleted: " + clubInDb.Name);
        }

        //    // POST club/
        [HttpPost]
        public async Task<IActionResult> New([FromBody] ClubDto clubDto)
        {
            var errorMsg = _clubService.Validate(clubDto);
            if (errorMsg != null)
                return BadRequest(errorMsg);

            var newClub = _clubService.CreateNew(clubDto);

            // Validate and Add Types
            var countryValMsg = _clubService.AddTypeR(newClub, clubDto);
            if (countryValMsg != null)
                return BadRequest(countryValMsg);

            // Validate and Add Region and/or club
            var countryValMsgg = _clubService.AddRegionCountriR(newClub, clubDto);
            if (countryValMsgg != null)
                return BadRequest(countryValMsgg);

            var saved = await _clubService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok(Mapper.Map<Club, ClubDto>(newClub));
        }
    }
}