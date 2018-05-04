using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                var tagInDb = _context.Tags.SingleOrDefault(c => c.Id == id);
                return tagInDb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public Tag ExistName(string name)
        {
            try
            {
                var tagInDb = _context.Tags.SingleOrDefault(t => t.Name.ToLower() == name.ToLower());
                return tagInDb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public string Validate(TagDto tagDto) {

            var checkName = CheckSameName(tagDto);
            if (checkName != null)
                return checkName;

            return string.IsNullOrEmpty(tagDto.Name) ? "You send a null or empty string!" : null;
        }

        public string CheckSameName(TagDto tagDto)
        {
            if (!string.IsNullOrEmpty(tagDto.Name))
            {
                if (_context.Tags.Any(t => t.Name.ToLower() == tagDto.Name.ToLower()))
                    return "A Tag with that name already exist!";
            }

            return null;
        }

        public string Compare(Tag tagInDb, TagDto tagDto)
        {
            if (!string.IsNullOrEmpty(tagDto.Name))
            {
                if (tagInDb.Name.ToLower() == tagDto.Name.ToLower())
                    return "Same name, not changes made";

            }

            return null;
        }

        public TagDto Edit(Tag tagInDb, TagDto tagDto)
        {

            if (Compare(tagInDb, tagDto) != null)
                return Mapper.Map<Tag, TagDto>(tagInDb);

            if (string.IsNullOrEmpty(tagDto.Name))
                return Mapper.Map<Tag, TagDto>(tagInDb);

            tagInDb.Name = tagDto.Name;

            return Mapper.Map<Tag, TagDto>(tagInDb);
        }

        public Tag CreateNew(TagDto tagDto)
        {
            var newTag = new Tag
            {
                Name = tagDto.Name
            };
            _context.Tags.Add(newTag);
            return newTag;
        }

        public bool Erase(Tag tagInDb)
        {
            try
            {
                _context.Tags.Remove(tagInDb);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

    }
}
