using AutoMapper;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;

namespace CMCore.Services
{
    public class CompanieService : GenericService<Companie, CompanieDto>, ICompanieService
    {
        public CompanieService(ContentManagerDbContext context) : base(context)
        {
        }

        public Companie CreateNew(CompanieDto companieDto)
        {
            var newCompanie = new Companie
            {
                Name = companieDto.Name
            };
            return AddEf(newCompanie) ? newCompanie : default(Companie);
        }

    }
}
