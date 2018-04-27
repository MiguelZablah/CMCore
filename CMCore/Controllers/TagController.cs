using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMCore.Controllers
{
    [Produces("application/json")]
    [Route("tag/")]
    [EnableCors("AllowSpecificOrigin")]
    public class TagController : Controller
    {
        private readonly ContentManagerDbContext _context;

        public TagController(ContentManagerDbContext context)
        {
            _context = context;
        }

        // GET tag/
        [HttpGet]
        public IActionResult Get(string name = null)
        {
            var tagsQuery = _context.Tags.ProjectTo<TagDto>();

            if (!String.IsNullOrWhiteSpace(name))
                tagsQuery = tagsQuery.Where(f => f.Name.ToLower().Contains(name));

            var tags = tagsQuery.ToList();

            if (tags.Count <= 0)
                return BadRequest("No Tags");

            return Ok(tags);
        }

        // GET tag/id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var tag = _context.Tags.ProjectTo<TagDto>().SingleOrDefault(c => c.Id == id);

            if (tag == null)
                return BadRequest("Tag not found");

            return Ok(tag);
        }

        // PATCH tag/id
        [HttpPatch("{id}")]
        public IActionResult Edit(int id, [FromBody] TagDto tagDto)
        {
            if (tagDto == null)
                return BadRequest("Did you realy send a tag?");

            var tagInDb = _context.Tags.SingleOrDefault(c => c.Id == id);

            if (tagInDb == null)
                return NotFound();

            if (!String.IsNullOrEmpty(tagDto.Name))
            {
                if (tagInDb.Name.ToLower() == tagDto.Name.ToLower())
                    return BadRequest("Same name, not changes made");

                if (_context.Tags.Any(t => t.Name.ToLower() == tagDto.Name.ToLower()))
                    return BadRequest("A Tag with that name already exist!");
            }

            // Keep name if not send
            if (String.IsNullOrEmpty(tagDto.Name))
                return BadRequest("You send a null or empty string!");

            Mapper.Map(tagDto, tagInDb);

            _context.SaveChanges();

            // Return new file
            var tag = _context.Tags.ProjectTo<TagDto>().SingleOrDefault(f => f.Id == tagInDb.Id);

            return Ok(tag);
        }

        // DELETE tag/id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var tagInDb = _context.Tags.SingleOrDefault(c => c.Id == id);

            if (tagInDb == null)
                return NotFound();

            _context.Tags.Remove(tagInDb);
            _context.SaveChanges();

            return Ok("Tag Deleted: " + id);
        }

        // POST tag/
        [HttpPost]
        public async Task<IActionResult> New([FromBody] TagDto tagDto)
        {
            if (tagDto == null)
                return BadRequest("Did you send one tag or somthing else?!");

            if (String.IsNullOrEmpty(tagDto.Name))
                return BadRequest("Not tag name send!");

            if (_context.Tags.Any(t => t.Name.ToLower() == tagDto.Name.ToLower()))
                return BadRequest("Tag name already exist! No duplicates plz!");

            var newTag = new Tag
            {
                Name = tagDto.Name
            };
            _context.Tags.Add(newTag);
            await _context.SaveChangesAsync();
            return Ok(Mapper.Map<Tag, TagDto>(newTag));
        }
    }
}