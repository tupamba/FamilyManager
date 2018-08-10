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
    }
    public class GroupFamilyRepository : IGroupFamilyRepository, IDisposable
    {
        private readonly DbModel db = null;
        public GroupFamilyRepository()
        {
            db = new DbModel();
        }
        public IUnitOfWork UnitOfWork
        {
            get { return db; }
        }
        public async Task<GroupFamily> AddGroupFamily(GroupFamily family)
        {
            var member = await db.MemberFamily.FirstOrDefaultAsync(x => x.UserName == family.Owner.UserName);
            if (member != null)
                family.Owner = member;

            family.MembersFamily = new List<MemberFamily>()
            { family.Owner};
            return await Task.FromResult<GroupFamily>(db.GroupFamily.Add(family));
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
