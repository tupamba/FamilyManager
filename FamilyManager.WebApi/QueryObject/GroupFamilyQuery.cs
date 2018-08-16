using FamilyManager.DataObject;
using FamilyManager.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FamilyManager.WebApi.QueryObject
{
    public class GroupFamilyQuery
    {
        private readonly DbModel model;
        public GroupFamilyQuery(DbModel context)
        {
            model = context;
        }
        public async Task<GroupFamily> GetforOwnerUser(string user)
        {
            return await model.GroupFamily.FirstOrDefaultAsync(x => x.Name == user);
        }
    }
}