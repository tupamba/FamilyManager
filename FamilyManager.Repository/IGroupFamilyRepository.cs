using FamilyManager.DataObject;
using FamilyManager.DataProvider;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace FamilyManager.Repository
{
    public interface IUnitOfWork : IDisposable
    {

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        Task<int> Commit();
    }
    public sealed class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The DbContext
        /// </summary>
        private DbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the UnitOfWork class.
        /// </summary>
        /// <param name="context">The object context</param>
        public UnitOfWork(DbContext context)
        {

            _dbContext = context;
        }



        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public Task<int> Commit()
        {
            // Save changes with the default options
            return _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }
    }
    public interface IGroupFamilyRepository: IDisposable
    {
        Task<GroupFamily> AddGroupFamily(GroupFamily family);
      //  IUnitOfWork UnitOfWork { get; }
        IEnumerable<K> QueryStoreExecute<K>(string query, Dictionary<string, string> paramsList);
        Task<GroupFamily> GetFamily(string name);
        Task<GroupFamily> GetFamilytoUser(string userName);
        Task<GroupFamily> GetFamilytoOwner(string ownerName);

    }
    public class GroupFamilyRepository : IGroupFamilyRepository, IDisposable
    {
        private readonly DbModel db = null;
        public GroupFamilyRepository(DbModel context)
        {
            db = context;
        }
        //public IUnitOfWork UnitOfWork
        //{
        //    get { return db; }
        //}
        public async Task<GroupFamily> AddGroupFamily(GroupFamily family)
        {
            var member = family.MembersFamily.FirstOrDefault();
            var dbmember = await db.MemberFamily.FirstOrDefaultAsync(x => x.UserName == member.UserName);
            if (dbmember != null)
            {
                dbmember.Owner = true;
                family.MembersFamily = new List<MemberFamily>()
                { dbmember};
            }

            return await Task.FromResult<GroupFamily>(db.GroupFamily.Add(family));
        }
        public async Task<GroupFamily> GetFamily(string name)
        {
            return await db.GroupFamily.FirstOrDefaultAsync(x => x.Name == name );
        }
        public async Task<GroupFamily> GetFamilytoUser(string userName)
        {
            return await db.GroupFamily.Include("MembersFamily").
                FirstOrDefaultAsync(x => x.MembersFamily != null? x.MembersFamily.Any(k => k.UserName == userName):false);
        }
        public async Task<GroupFamily> GetFamilytoOwner(string ownerName)
        {
            return await db.GroupFamily.Include("MembersFamily").
                FirstOrDefaultAsync(x => x.MembersFamily.Any(k => k.UserName == ownerName && k.Owner));
        }
        public void Dispose()
        {
            if(db != null)
                db.Dispose();
        }

        public IEnumerable<K> QueryStoreExecute<K>(string query, Dictionary<string, string> paramsList)
        {
            return db.QueryStoreExecute<K>(query, paramsList);
        }
    }
}
