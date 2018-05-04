using System.Collections.Generic;
using System.Threading.Tasks;
using CMCore.DTO;
using CMCore.Models;

namespace CMCore.Interfaces
{
    public interface ITagService
    {
        List<TagDto> FindAll(string name);
        Tag Exist(int id);
        TagDto Edit(Tag tagInDb, TagDto tagDto);
        string Validate(TagDto tagDto);
        string CheckSameName(TagDto tagDto);
        string Compare(Tag tagInDb, TagDto tagDto);
        Tag CreateNew(TagDto tagDto);
        bool Erase(Tag tagInDb);
    }
}
