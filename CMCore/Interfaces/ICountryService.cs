﻿using System.Linq;
using System.Threading.Tasks;
using CMCore.DTO;
using CMCore.Models;

namespace CMCore.Interfaces
{
    public interface ICountryService
    {
        IQueryable<Countrie> FindAll(string name);
        IQueryable<Countrie> Exist(int id);
        IQueryable<Countrie> ExistName(string name);
        string Validate(CountrieDto countrieDto);
        string CheckSameName(CountrieDto countriDto);
        bool Erase(Countrie countrieInDb);
        Task<bool> SaveEf();
        CountrieDto Edit(Countrie countrieInDb, CountrieDto countrieDto);
        Countrie CreateNew(CountrieDto countrieDto);
        string AddRegionR(CountrieDto countrieDto, Region regionInDb);
    }
}
