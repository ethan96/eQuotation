using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using LinqKit;

namespace eQuotation.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        internal AppDbContext dbContext;
        internal DbSet<T> dbSet;

        public Repository(AppDbContext context)
        {
            this.dbContext = context;
            this.dbSet = context.Set<T>();
        }

        public IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            foreach(var includeProperty in includeProperties.Split
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

        public T GetFirst(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<TResult> GetFields<TResult>(Expression<Func<T, TResult>> columns, Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            return query.Select(columns).ToList();
        }

        public int Count(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            return query.Count();
        }

        public bool Exists(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                if (Count(filter) > 0) return true;
            }
                
            return false;
        }

        public T GetByID(string id)
        {
            return dbSet.Find(id);
        }

        public void Insert(T entity)
        {
            dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            dbSet.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void DeleteByID(string id)
        {
            T entity = dbSet.Find(id);
            Delete(entity);
        }

        public void Delete(T entity)
        {
            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public void DeleteAll(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach(var ent in query)
            {
                Delete(ent);
            }
        }

        private string CreateNewID(Int32 id, int len, string prefix)
        {
            var newID = id.ToString();

            while (newID.Length < len)
                newID = string.Format("0{0}", newID);

            if (!string.IsNullOrEmpty(prefix))
                newID = string.Format("{0}{1}", prefix, newID);

            return string.Format("{0}", newID.ToString());
        }

        public string NewID(Expression<Func<T, string>> filter = null, int len = 8, string prefix = null)
        {
            Int32 lastCount = 1;
            string rowID = string.Empty;
            string newKey = string.Empty;
            string entity = typeof(T).ToString();
            int prevId = 0;

            //get last inputed ID for given table
            var lastID = dbSet.Max(filter);

            if (lastID != null && lastID.Count() > 0)
            {
                if (!string.IsNullOrEmpty(prefix))
                    lastID = lastID.Replace(prefix, "");

                prevId = Convert.ToInt32(lastID);
            }

            //increment key
            lastCount = Convert.ToInt32(prevId) + 1;
            newKey = CreateNewID(lastCount, len, prefix);

            //check wheter key has been reserved
            var strKey = string.Format("{0}-{1}", entity, newKey);
            var outKey = newKey;
            while (dbContext.ReservedKeys.TryGetValue(strKey, out outKey))
            {
                lastCount++;
                newKey = CreateNewID(lastCount, len, prefix);
                strKey = string.Format("{0}-{1}", entity, newKey);
                outKey = newKey;
            }

            //reserve key
            dbContext.ReservedKeys.Add(strKey, newKey);

            //initiate newID
            return newKey;
        }
    }
}