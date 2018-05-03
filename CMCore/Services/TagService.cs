using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;

namespace CMCore.Services
{
    public class TagService : ITagService
    {
        private readonly ContentManagerDbContext _context;

        public TagService(ContentManagerDbContext context)
        {
            _context = context;
        }
       
        public List<TagDto> FindAll(string name)
        {
            var tagsQuery = _context.Tags.ProjectTo<TagDto>();

            if (!string.IsNullOrWhiteSpace(name))
                tagsQuery = tagsQuery.Where(f => f.Name.ToLower().Contains(name));

            var tags = tagsQuery.ToList();

            if(tags.Count <= 0)
                return null;

            return tags;
        }

        public Tag Exist(int id)
        {
            var tagInDb = _context.Tags.SingleOrDefault(c => c.Id == id);

            return tagInDb;
        }

        public string Validate(TagDto tagDto, Tag tagInDb = null)
        {

            if (!string.IsNullOrEmpty(tagDto.Name))
            {
                if (tagInDb != null && tagInDb.Name.ToLower() == tagDto.Name.ToLower())
                    return "Same name, not changes made";

                if (_context.Tags.Any(t => t.Name.ToLower() == tagDto.Name.ToLower()))
                    return "A Tag with that name already exist!";
            }

            return string.IsNullOrEmpty(tagDto.Name) ? "You send a null or empty string!" : null;
        }

        public TagDto Edit(Tag tagInDb, TagDto tagDto)
        {

            Mapper.Map(tagDto, tagInDb);

            _context.SaveChanges();

            var tag = _context.Tags.ProjectTo<TagDto>().SingleOrDefault(f => f.Id == tagInDb.Id);

            return tag;
        }

        public async Task<TagDto> SaveNew(TagDto tagDto)
        {
            var newTag = new Tag
            {
                Name = tagDto.Name
            };
            _context.Tags.Add(newTag);
            await _context.SaveChangesAsync();
            return Mapper.Map<Tag, TagDto>(newTag);
        }

        public bool Erase(Tag tagInDb)
        {
            _context.Tags.Remove(tagInDb);
            _context.SaveChanges();

            return true;
        }

    }
}
