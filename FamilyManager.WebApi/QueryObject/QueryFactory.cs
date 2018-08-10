using FamilyManager.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyManager.WebApi.QueryObject
{
    public interface IQueryFactory : IDisposable
    {
        GroupFamilyQuery GetGroupFamilyQuery();
    }
        public class QueryFactory : IQueryFactory, IDisposable
    {
        private readonly DbModel model;
        public QueryFactory(DbModel context)
        {
            model = context;
        }

        public void Dispose()
        {
            if (model != null)
                model.Dispose();
        }

        public GroupFamilyQuery GetGroupFamilyQuery()
        {
            return new GroupFamilyQuery(model);
        }
    }
}