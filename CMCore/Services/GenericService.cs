using System;
using System.Linq;
using System.Threading.Tasks;
using CMCore.Data;
using CMCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMCore.Services
{
    public class GenericService<T, TDto> where T : class, IEntity where TDto : class, IEntity
    {
        private readonly ContentManagerDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericService(ContentManagerDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> FindAll(string name)
        {

            if (string.IsNullOrWhiteSpace(name))
                return _dbSet.AsQueryable();

            return _dbSet.Where(f => f.Name.ToLower().Contains(name)).AsQueryable();

        }

        public IQueryable<T> Exist(int id)
        {
            try
            {
                var resultInDb = _dbSet.Where(c => c.Id == id).AsQueryable();
                return resultInDb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return _dbSet.AsQueryable();
            }
        }

        public IQueryable<T> ExistName(string name)
        {
            try
            {
                var resultInDb = _dbSet.Where(t => t.Name.ToLower() == name.ToLower()).AsQueryable();
                return resultInDb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return _dbSet.AsQueryable();
            }
        }

        public string Validate(TDto tDto)
        {

            var checkName = CheckSameName(tDto);
            if (checkName != null)
                return checkName;

            return string.IsNullOrWhiteSpace(tDto.Name) ? "You send am invalid name!" : null;
        }

        public string CheckSameName(TDto tDto)
        {
            if (string.IsNullOrWhiteSpace(tDto.Name))
                return null;

            if (!string.IsNullOrWhiteSpace(tDto.Name))
            {
                if (_dbSet.Any(t => t.Name.ToLower() == tDto.Name.ToLower()))
                    return "That name already exist!";
            }

            return null;
        }

        public string Compare(T tInDb, TDto tDto)
        {
            if (!string.IsNullOrWhiteSpace(tInDb.Name))
            {
                if (tDto.Name.ToLower() == tInDb.Name.ToLower())
                    return "Same name, not changes made";

            }

            return null;
        }

        public bool Erase(T tInDb)
        {
            try
            {
                _dbSet.Remove(tInDb);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool AddEf(T tInDb)
        {
            try
            {
                _dbSet.Add(tInDb);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> SaveEf()
        {
            try
            {
                await _context.SaveChangesAsync();
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
