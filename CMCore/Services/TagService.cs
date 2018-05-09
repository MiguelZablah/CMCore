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

        public Tag CreateNew(TagDto tagDto)
        {
            var newTag = new Tag
            {
                Name = tagDto.Name
            };

            return AddEf(newTag) ? newTag : default(Tag);
        }

    }
}
