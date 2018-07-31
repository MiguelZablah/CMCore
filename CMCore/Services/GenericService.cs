using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMCore.Data;
using CMCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMCore.Services
{
    public class GenericService<T, TDto> where T : class, IEntity where TDto : class, IEntity
    {
        protected readonly ContentManagerDbContext Context;
        private readonly DbSet<T> _dbSet;

        public GenericService(ContentManagerDbContext context)
        {
            Context = context;
            _dbSet = Context.Set<T>();
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

        private IQueryable<T> Query(bool includes)
        {
            var query = _dbSet.AsQueryable();
            if (includes)
            {
                foreach (var property in Context.Model.FindEntityType(typeof(T)).GetNavigations())
                    query = query.Include(property.Name);
            }
            return query;
        }

        public IQueryable<T> ExistName(string name, bool getIncludes = false)
        {
            try
            {
                var resultInDb = Query(getIncludes).Where(t => t.Name.Equals(name.ToLower()));
                return resultInDb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return _dbSet.AsQueryable();
            }
        }

        public virtual string Validate(TDto tDto)
        {
            var checkName = CheckSameName(tDto.Name);
            if (checkName != null)
                return checkName;

            return string.IsNullOrWhiteSpace(tDto.Name) ? "You send an invalid name!" : null;
        }

        public string CheckSameName(string checkName)
        {
            if (string.IsNullOrWhiteSpace(checkName))
                return null;

            if (_dbSet.Any(t => t.Name.Equals(checkName.ToLower())))
                return "That name already exist!";

            return null;
        }

        public bool CompareString(string a, string b)
        {
            if (string.IsNullOrWhiteSpace(a) || string.IsNullOrWhiteSpace(b))
                return false;

            return !a.Equals(b);
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
                _dbSet.AddAsync(tInDb);
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
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public virtual TDto Edit(T tInDb, TDto tDto)
        {
            if (CompareString(tInDb.Name, tDto.Name))
                tInDb.Name = tDto.Name;

            return Mapper.Map<T, TDto>(tInDb);
        }

    }

}
