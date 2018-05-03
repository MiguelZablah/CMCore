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
    [Route("Companie/")]
    [EnableCors("AllowSpecificOrigin")]
    public class CompanieController : Controller
    {
        private readonly ICompanieService _companieService;

        public CompanieController(ICompanieService companieService)
        {
            _companieService = companieService;
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
            {
                return BadRequest("Companie dosen't exist!");
            }

            return Ok(Mapper.Map<Companie, CompanieDto>(companieInDb));
        }

        // PATCH companie/id
        [HttpPatch("{id}")]
        public IActionResult Edit(int id, [FromBody] CompanieDto companieDto)
        {
            var companieInDb = _companieService.Exist(id);
            if (companieInDb == null)
            {
                return BadRequest("Companie dosen't exist!");
            }

            var errorMsg = _companieService.Validate(companieDto);
            if (errorMsg != null)
            {
                return BadRequest(errorMsg);
            }

            var errMsg = _companieService.Compare(companieInDb, companieDto);
            if (errMsg != null)
            {
                return BadRequest(errMsg);
            }

            var companieSave = _companieService.Edit(companieInDb, companieDto);

            return Ok(companieSave);
        }

        // DELETE companie/id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var companieInDb = _companieService.Exist(id);
            if (companieInDb == null)
            {
                return BadRequest("Companie dosen't exist!");
            }

            var delete = _companieService.Erase(companieInDb);
            if (!delete)
            {
                return BadRequest("Companie not deleted!");
            }

            return Ok("Companie Deleted: " + companieInDb.Name);
        }

        // POST companie/
        [HttpPost]
        public async Task<IActionResult> New([FromBody] CompanieDto companieDto)
        {
            var errorMsg = _companieService.Validate(companieDto);
            if (errorMsg != null)
            {
                return BadRequest(errorMsg);
            }

            var newCompanie = await _companieService.SaveNew(companieDto);
            return Ok(newCompanie);
        }
    }
}