using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMCore.Data;
using CMCore.DTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Type = CMCore.Models.Type;

namespace CMCore.Controllers
{
    [Produces("application/json")]
    [Route("type/")]
    [EnableCors("AllowSpecificOrigin")]
    public class TypeController : Controller
    {
        private readonly ContentManagerDbContext _context;

        public TypeController(ContentManagerDbContext context)
        {
            _context = context;
        }

        // GET type/
        [HttpGet]
        public IActionResult Get(string name = null)
        {
            var typesQuery = _context.Types.ProjectTo<TypeDto>();

            if (!String.IsNullOrWhiteSpace(name))
                typesQuery = typesQuery.Where(f => f.Name.ToLower().Contains(name));

            var types = typesQuery.ToList();

            if (types.Count <= 0)
                return BadRequest("No Types");

            return Ok(types);
        }

        // GET type/id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var type = _context.Types.ProjectTo<TypeDto>().SingleOrDefault(t => t.Id == id);

            if (type == null)
                return BadRequest("Type not found");

            return Ok(type);
        }

        // PATCH /type/id
        [HttpPatch("{id}")]
        public IActionResult Edit(int id, [FromBody] TypeDto typeDto)
        {
            var typeInDb = _context.Types.SingleOrDefault(t => t.Id == id);

            if (typeInDb == null)
                return NotFound();

            if (!String.IsNullOrEmpty(typeDto.Name))
            {
                if (typeInDb.Name.ToLower() == typeDto.Name.ToLower())
                    return BadRequest("Same name, not changes made");

                if (_context.Types.Any(t => t.Name.ToLower() == typeDto.Name.ToLower()))
                    return BadRequest("A type with that name already exist!");
            }

            // Keep name if not send
            if (String.IsNullOrEmpty(typeDto.Name))
                typeDto.Name = typeInDb.Name;

            Mapper.Map(typeDto, typeInDb);

            _context.SaveChanges();

            // Return new file
            var type = _context.Types.ProjectTo<TypeDto>().SingleOrDefault(t => t.Id == typeInDb.Id);

            return Ok(type);
        }

        // DELETE type/delete/id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var typeInDb = _context.Types.SingleOrDefault(c => c.Id == id);

            if (typeInDb == null)
                return NotFound();

            _context.Types.Remove(typeInDb);
            _context.SaveChanges();

            return Ok("Type Deleted: " + id);
        }

        // POST type/new
        [HttpPost]
        public async Task<IActionResult> New([FromBody] TypeDto typeDto)
        {
            if (typeDto == null)
                return BadRequest("Did you send one type or somthing else?!");

            if (String.IsNullOrEmpty(typeDto.Name))
                return BadRequest("Not type name send!");

            if (_context.Types.Any(t => t.Name.ToLower() == typeDto.Name.ToLower()))
                return BadRequest("Type name already exist! No duplicates plz!");

            var newType = new Type
            {
                Name = typeDto.Name
            };
            _context.Types.Add(newType);
            await _context.SaveChangesAsync();
            return Ok(Mapper.Map<Type, TypeDto>(newType));
        }
    }
}