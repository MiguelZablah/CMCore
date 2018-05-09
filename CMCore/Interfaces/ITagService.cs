using System.Linq;
using System.Threading.Tasks;
using CMCore.DTO;
using CMCore.Models;

namespace CMCore.Interfaces
{
    public interface ITagService
    {
        IQueryable<Tag> FindAll(string name);
        IQueryable<Tag> Exist(int id);
        string Validate(TagDto tagDto);
        string CheckSameName(TagDto tagDto);
        string Compare(Tag tagInDb, TagDto tagDto);
        bool Erase(Tag tagInDb);
        Task<bool> SaveEf();
        TagDto Edit(Tag tagInDb, TagDto tagDto);
        Tag CreateNew(TagDto tagDto);
    }
}
