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
    public interface IGroupFamilyRepository: IDisposable
    {
        Task<GroupFamily> AddGroupFamily(GroupFamily family);
        IUnitOfWork UnitOfWork { get; }
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
        public IUnitOfWork UnitOfWork
        {
            get { return db; }
        }
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
                FirstOrDefaultAsync(x => x.MembersFamily.Any(k => k.UserName == userName));
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
