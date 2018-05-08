using AutoMapper;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;

namespace CMCore.Services
{
    public class TagService : GenericService<Tag, TagDto>, ITagService
    {
        public TagService(ContentManagerDbContext context) : base(context)
        {
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
            DbSet.Add(newTag);
            return newTag;
        }

    }
}
