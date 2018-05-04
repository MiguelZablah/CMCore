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
    [Route("Companie/")]
    [EnableCors("AllowSpecificOrigin")]
    public class CompanieController : Controller
    {
        private readonly ICompanieService _companieService;
        private readonly IEfService _efService;

        public CompanieController(ICompanieService companieService, IEfService efService)
        {
            _companieService = companieService;
            _efService = efService;
        }

        // GET companie/
        [HttpGet]
        public IActionResult Get(string name = null)
        {
            var companies = _companieService.FindAll(name);

            if (companies == null)
                return BadRequest("No Companies");

            return Ok(companies);
        }

        // GET companie/id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var companieInDb = _companieService.Exist(id);
            if (companieInDb == null)
                return BadRequest("Companie dosen't exist!");

            return Ok(Mapper.Map<Companie, CompanieDto>(companieInDb));
        }

        // PATCH companie/id
        [HttpPatch("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] CompanieDto companieDto)
        {
            if (companieDto == null)
                return BadRequest("You send a empty countrie");

            var companieInDb = _companieService.Exist(id);
            if (companieInDb == null)
                return BadRequest("Companie dosen't exist!");

            var errorMsg = _companieService.CheckSameName(companieDto);
            if (errorMsg != null)
                return BadRequest(errorMsg);

            var newCompanie = _companieService.Edit(companieInDb, companieDto);

            var saved = await _efService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok(Mapper.Map<Companie, CompanieDto>(_companieService.Exist(newCompanie.Id)));
        }

        // DELETE companie/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var companieInDb = _companieService.Exist(id);
            if (companieInDb == null)
                return BadRequest("Companie dosen't exist!");

            var delete = _companieService.Erase(companieInDb);
            if (!delete)
                return BadRequest("Companie not deleted!");

            var saved = await _efService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok("Companie Deleted: " + companieInDb.Name);
        }

        // POST companie/
        [HttpPost]
        public async Task<IActionResult> New([FromBody] CompanieDto companieDto)
        {
            var errorMsg = _companieService.Validate(companieDto);
            if (errorMsg != null)
                return BadRequest(errorMsg);

            var newCompanie = _companieService.CreateNew(companieDto);

            var saved = await _efService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok(Mapper.Map<Companie, CompanieDto>(newCompanie));
        }
    }
}