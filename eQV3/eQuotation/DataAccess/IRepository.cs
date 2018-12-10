using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eQuotation.DataAccess
{
    public interface IRepository<T>
    {
        //get entity using filter
        IEnumerable<T> Get( Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");

        //get entity using filter
        T GetFirst(Expression<Func<T, bool>> filter = null, string includeProperties = "");

        //get specific fields
        IEnumerable<TResult> GetFields<TResult>(Expression<Func<T, TResult>> columns, Expression<Func<T, bool>> filter = null);
        
        //get entity by ID
        T GetByID(string id);

        //insert new entity
        void Insert(T entity);

        //update entity
        void Update(T entity);

        //delete entity by ID
        void DeleteByID(string id);

        //delete an entity
        void Delete(T entity);

        void DeleteAll(Expression<Func<T, bool>> filter = null);

        //create sequence unique ID
        string NewID(Expression<Func<T, string>> filter = null, int len = 8, string prefix = null);

        //count records
        int Count(Expression<Func<T, bool>> filter = null); 

        //check if record exists
        bool Exists(Expression<Func<T, bool>> filter = null);
    }
}
