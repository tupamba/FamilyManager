using FamilyManager.DataObject;
using FamilyManager.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace FamilyManager.WebApi.Command
{
    public class AddGroupFamilyCommand : IRequest<int>
    {
        public GroupFamily GroupFamily { get; set; }
        public string  UserName { get; set; }
        public AddGroupFamilyCommand(GroupFamily group, string user)
        {
            UserName = user;
            GroupFamily = group;
        }
    }
    public class AddGroupFamilyCommandHandler : IRequestHandler<AddGroupFamilyCommand, int>, IDisposable
    {
        private readonly IGroupFamilyRepository _respository;
        public AddGroupFamilyCommandHandler(IGroupFamilyRepository respo)
        {
            _respository = respo;
        }

        public void Dispose()
        {
            if (_respository != null)
                _respository.Dispose();
        }

        //public AddGroupFamilyCommandHandler()
        //{
        //}
        public async Task<int> Execute(AddGroupFamilyCommand cmd)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("Name", cmd.GroupFamily.Name);
            data.Add("Owner", cmd.UserName);
            var result = _respository.QueryStoreExecute<int>("SelectFamily @Name, @Owner", data);
            if (result.FirstOrDefault() == 0)
            {
                cmd.GroupFamily.Owner = new MemberFamily() { UserName = cmd.UserName };
                await _respository.AddGroupFamily(cmd.GroupFamily);
                return await _respository.UnitOfWork.SaveChangesAsync();
            }
            else
                return result.FirstOrDefault();
        }

        public Task<int> Handle(AddGroupFamilyCommand request, CancellationToken cancellationToken)
        {
            
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("Name", request.GroupFamily.Name);
            data.Add("Owner", request.UserName);
            var result = _respository.QueryStoreExecute<int>("SelectFamily @Name, @Owner", data);
            if (result.FirstOrDefault() == 0)
            {
                request.GroupFamily.Owner = new MemberFamily() { UserName = request.UserName };
                _respository.AddGroupFamily(request.GroupFamily);
                return _respository.UnitOfWork.SaveChangesAsync();
            }
            else
                return Task.FromResult(result.FirstOrDefault());
        }
    }
}
