using System.Linq;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;
using CMCore.Models.RelacionClass;

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

        public string AddFileR(TagDto tagDto, File fileInDb)
        {
            var tagInDb = ExistName(tagDto.Name).FirstOrDefault();
            if (tagInDb == null)
            {
                var createdTag = CreateNew(tagDto);
                if (createdTag == null)
                    return "Tag not created!";

                var newTag = createdTag;
                var newFileTag = new FileTag
                {
                    FileId = fileInDb.Id,
                    TagId = newTag.Id
                };
                Context.FileTags.Add(newFileTag);

                return null;
            }

            if (string.IsNullOrEmpty(tagDto.Name))
                return "You send a null or empty Tag!";

            var typeHasClub = fileInDb.FileTags.Any(cr => cr.TagId == tagInDb.Id);
            if (!typeHasClub)
            {
                var newFileTag = new FileTag
                {
                    FileId = fileInDb.Id,
                    TagId = tagInDb.Id
                };
                Context.FileTags.Add(newFileTag);
            }

            return null;
        }

    }
}
