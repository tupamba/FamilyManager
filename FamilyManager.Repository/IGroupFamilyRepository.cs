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
    public interface IGroupFamilyRepository
    {
        Task<GroupFamily> AddGroupFamily(GroupFamily family);
        IUnitOfWork UnitOfWork { get; }
        Task<IEnumerable<K>> QueryStoreExecute<K>(string query, Dictionary<string, string> paramsList);
    }
    public class GroupFamilyRepository : IGroupFamilyRepository
    {
        private readonly DbModel db = null;
        public GroupFamilyRepository(DbModel model)
        {
            db = model;
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
        public async Task<IEnumerable<K>> QueryStoreExecute<K>(string query, Dictionary<string, string> paramsList)
        {
            return await db.QueryStoreExecute<K>(query, paramsList);
        }
    }
}
