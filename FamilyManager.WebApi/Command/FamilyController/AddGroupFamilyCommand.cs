using FamilyManager.DataObject;
using FamilyManager.Repository;
using FamilyManager.WebApi.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace FamilyManager.WebApi.Command.FamilyController
{
    public class AddGroupFamilyCommand : IRequest<ResponseFamilyErrorEnum>
    {
        public GroupFamily GroupFamily { get; set; }
        public string  UserName { get; set; }
        public AddGroupFamilyCommand(GroupFamily group, string user)
        {
            UserName = user;
            GroupFamily = group;
        }
    }
    public class AddGroupFamilyCommandHandler : IRequestHandler<AddGroupFamilyCommand, ResponseFamilyErrorEnum>, IDisposable
    {
        private readonly IGroupFamilyRepository _respository;
        private readonly IUnitOfWork _unitOfWork;
        public AddGroupFamilyCommandHandler(IGroupFamilyRepository respo, IUnitOfWork unitOfWork)
        {
            _respository = respo;
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            if (_respository != null)
                _respository.Dispose();
        }

        //Dictionary<string, string> data = new Dictionary<string, string>();
        //data.Add("Name", cmd.GroupFamily.Name);
        //    data.Add("Owner", cmd.UserName);
        //    var result = _respository.QueryStoreExecute<int>("ValidateAddFamily @Name, @Owner", data);
        public async Task<ResponseFamilyErrorEnum> Handle(AddGroupFamilyCommand request, CancellationToken cancellationToken)
        {

            if (await _respository.GetFamily(request.GroupFamily.Name) != null)
                return ResponseFamilyErrorEnum.FamilyNameDuplicate;
            else if (await _respository.GetFamilytoUser(request.UserName) != null)
                return ResponseFamilyErrorEnum.UserFamilyAlredyExist;
            else
            {
                var member = new MemberFamily(request.UserName, true);
                request.GroupFamily.MembersFamily = new List<MemberFamily>()
                { member}; 
                var add = await _respository.AddGroupFamily(request.GroupFamily);
                var res = await _unitOfWork.Commit();
                return res > 0 ? ResponseFamilyErrorEnum.Ok : ResponseFamilyErrorEnum.Error;
            }
        }
    }
}
