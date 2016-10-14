using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ComicsReadProgress
{
    public static class Repository
    {
        private static readonly Context Context = new Context();

        public static IQueryable<TEntity> Select<TEntity>() where TEntity : class
        {
            return Context.Set<TEntity>();
        }

        public static void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            Context.Entry(entity).State = EntityState.Added;
            Context.SaveChanges();
        }

        public static void Inserts<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Context.Configuration.AutoDetectChangesEnabled = false;
            Context.Configuration.ValidateOnSaveEnabled = false;
            foreach (var entity in entities)
                Context.Entry(entity).State = EntityState.Added;
            Context.SaveChanges();
            Context.Configuration.AutoDetectChangesEnabled = true;
            Context.Configuration.ValidateOnSaveEnabled = true;
        }

        public static void Update<TEntity>(TEntity entity) where TEntity : class
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public static void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            Context.Entry(entity).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public static void ClearTable(string tableName) 
        {
            Context.Database.ExecuteSqlCommand($"delete from {tableName}");
        }
    }
}