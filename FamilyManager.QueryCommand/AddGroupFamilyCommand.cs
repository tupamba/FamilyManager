using FamilyManager.DataObject;
using FamilyManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyManager.QueryCommand
{
    public class AddGroupFamilyCommand
    {
        public GroupFamily GroupFamily { get; set; }
        public string  UserName { get; set; }
        public AddGroupFamilyCommand(GroupFamily group, string user)
        {
            UserName = user;
            GroupFamily = group;
        }
    }
    public class AddGroupFamilyCommandHandler
    {
        private readonly IGroupFamilyRepository _respository;
        public AddGroupFamilyCommandHandler(IGroupFamilyRepository respo)
        {
            _respository = respo;
        }

        public async Task<int> Execute(AddGroupFamilyCommand cmd)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("Name", cmd.GroupFamily.Name);
            data.Add("Owner", cmd.UserName);
            var result = await _respository.QueryStoreExecute<int>("SelectFamily @Name, @Owner", data);
            if (result.FirstOrDefault() == 0)
            {
                cmd.GroupFamily.Owner = new MemberFamily() { UserName = cmd.UserName };
                await _respository.AddGroupFamily(cmd.GroupFamily);
                return await _respository.UnitOfWork.SaveChangesAsync();
            }
            else
                return result.FirstOrDefault();
        }
    }
}
