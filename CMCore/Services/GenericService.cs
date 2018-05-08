using System;
using System.Linq;
using CMCore.Data;
using CMCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMCore.Services
{
    public class GenericService<T, TDto> where T : class, IEntity where TDto : class, IEntity
    {
        protected readonly DbSet<T> DbSet;

        public GenericService(ContentManagerDbContext context)
        {
            DbSet = context.Set<T>();
        }

        public IQueryable<T> FindAll(string name)
        {

            if (string.IsNullOrWhiteSpace(name))
                return DbSet.AsQueryable();

            return DbSet.Where(f => f.Name.ToLower().Contains(name)).AsQueryable();

        }

        public T Exist(int id)
        {
            try
            {
                var resultInDb = DbSet.SingleOrDefault(c => c.Id == id);
                return resultInDb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default(T);
            }
        }

        public T ExistName(string name)
        {
            try
            {
                var tagInDb = DbSet.SingleOrDefault(t => t.Name.ToLower() == name.ToLower());
                return tagInDb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default(T);
            }
        }

        public string Validate(TDto tDto)
        {

            var checkName = CheckSameName(tDto);
            if (checkName != null)
                return checkName;

            return string.IsNullOrWhiteSpace(tDto.Name) ? "You send a null or empty string!" : null;
        }

        public string CheckSameName(TDto tDto)
        {
            if (!string.IsNullOrWhiteSpace(tDto.Name))
            {
                if (DbSet.Any(t => t.Name.ToLower() == tDto.Name.ToLower()))
                    return "That name already exist!";
            }

            return Enumerable.Empty<T>().ToString();
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
                DbSet.Remove(tInDb);
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
