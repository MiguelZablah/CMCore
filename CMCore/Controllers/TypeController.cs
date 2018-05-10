using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMCore.DTO;
using CMCore.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Type = CMCore.Models.Type;

namespace CMCore.Controllers
{
    [Produces("application/json")]
    [Route("type/")]
    [EnableCors("AllowSpecificOrigin")]
    public class TypeController : Controller
    {
        private readonly ITypeService _typeService;

        public TypeController(ITypeService typeService)
        {
            _typeService = typeService;
        }

        // GET type/
        [HttpGet]
        public IActionResult Get(string name = null)
        {
            var types = _typeService.FindAll(name).ProjectTo<TypeDto>().ToList();
            if (!types.Any())
                return BadRequest("No Types");

            return Ok(types);
        }

        // GET type/id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var typeInDb = _typeService.Exist(id).ProjectTo<TagDto>().FirstOrDefault();
            if (typeInDb == null)
                return BadRequest("Type dosen't exist!");

            return Ok(typeInDb);
        }

        // PATCH /type/id
        [HttpPatch("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] TypeDto typeDto)
        {
            if (typeDto == null)
                return BadRequest("You send a empty countrie");

            var typeInDb = _typeService.Exist(id).FirstOrDefault();
            if (typeInDb == null)
                return BadRequest("Type dosen't exist!");

            var errorMsg = _typeService.CheckSameName(typeDto.Name);
            if (errorMsg != null)
                return BadRequest(errorMsg);

            var newType = _typeService.Edit(typeInDb, typeDto);

            var saved = await _typeService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok(_typeService.Exist(newType.Id).ProjectTo<TagDto>().FirstOrDefault());
        }

        // DELETE type/delete/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var typeInDb = _typeService.Exist(id).FirstOrDefault();
            if (typeInDb == null)
                return BadRequest("Type dosen't exist!");

            var delete = _typeService.Erase(typeInDb);
            if (!delete)
                return BadRequest("Type not deleted!");

            var saved = await _typeService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok("Type Deleted: " + typeInDb.Name);
        }

        // POST type/new
        [HttpPost]
        public async Task<IActionResult> New([FromBody] TypeDto typeDto)
        {
            var errorMsg = _typeService.Validate(typeDto);
            if (errorMsg != null)
                return BadRequest(errorMsg);

            var newType = _typeService.CreateNew(typeDto);

            var saved = await _typeService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok(Mapper.Map<Type, TypeDto>(newType));
        }
    }
}