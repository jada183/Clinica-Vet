using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Windows;

namespace ClinicaVeterinaria.DAL
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        protected Context context;
        DbSet<TEntity> dbSet;

        public GenericRepository(Context context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        public void actualizar(TEntity entidad)
        {
            
            context.Entry(entidad).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void eliminar(TEntity entidad)
        {
            //dbSet.Remove(entidad);
            
            
                context.Entry(entidad).State = EntityState.Deleted;
                context.SaveChanges();
          
            
        }
        public void eliminarVarios(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = dbSet.Where(predicate).ToList();
            entities.ForEach(x => context.Entry(x).State = EntityState.Deleted);
            context.SaveChanges();
        }
        public void crear(TEntity entidad)
        {
            dbSet.Add(entidad);
            context.SaveChanges();
        }
        public List<TEntity> obtenerTodos()
        {
            return context.Set<TEntity>().ToList();
        }

        public TEntity obtenerUno(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.FirstOrDefault(predicate);
        }
        public List<TEntity> obtenerVarios(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate).ToList();
        }
        public virtual IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "")
        {
            dbSet = context.Set<TEntity>();
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
    }
}
