using FamilyManager.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Collections;

namespace FamilyManager.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class, IDisposable
    {
        private DbModel db = null;
        public RepositoryBase(DbModel model)
        {
            db = model;
        }
        public void Create(T entity)
        {
            this.db.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            this.db.Set<T>().Remove(entity);
        }
        public async Task<IEnumerable<K>> QueryExecuteAsync<K>(string query, Dictionary<string,string> paramsList)
        {
            if (paramsList?.Count > 0)
            {
                List<SqlParameter> sqlparams = new List<SqlParameter>();
                foreach (var item in paramsList)
                {
                    SqlParameter param = new SqlParameter("@" + item.Key, item.Value);
                    sqlparams.Add(param);
                }
                return await this.db.Database.SqlQuery<K>(query, sqlparams.ToArray()).ToListAsync();
            }
            else
                return null;
          
        }
        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await this.db.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByConditionAync(Expression<Func<T, bool>> expression)
        {
            return await this.db.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await this.db.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }

     
    }
}
