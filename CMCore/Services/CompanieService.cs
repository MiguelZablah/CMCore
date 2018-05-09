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

        public CompanieDto Edit(Companie companieInDb, CompanieDto companieDto)
        {

            if (Compare(companieInDb, companieDto) != null)
                return Mapper.Map<Companie, CompanieDto>(companieInDb);

            if (string.IsNullOrEmpty(companieDto.Name))
                return Mapper.Map<Companie, CompanieDto>(companieInDb);

            companieInDb.Name = companieDto.Name;

            return Mapper.Map<Companie, CompanieDto>(companieInDb);
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
