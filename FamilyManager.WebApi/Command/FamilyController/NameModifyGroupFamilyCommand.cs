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
    public class NameModifyGroupFamilyCommand : IRequest<ResponseFamilyErrorEnum>
    {
        public string UserName { get; set; }
        public string  Name { get; set; }
        public NameModifyGroupFamilyCommand(string name, string userName)
        {
            Name = name;
            UserName = userName;
        }
    }
    public class NameModifyGroupFamilyCommandCommandHandler : IRequestHandler<NameModifyGroupFamilyCommand, ResponseFamilyErrorEnum>, IDisposable
    {
        private readonly IGroupFamilyRepository _respository;
        private readonly IUnitOfWork _unitOfWork;
        public NameModifyGroupFamilyCommandCommandHandler(IGroupFamilyRepository respo, IUnitOfWork unitOfWork)
        {
            _respository = respo;
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            if (_respository != null)
                _respository.Dispose();
        }
        public async Task<ResponseFamilyErrorEnum> Handle(NameModifyGroupFamilyCommand request, CancellationToken cancellationToken)
        {

            GroupFamily family = await _respository.GetFamilytoOwner(request.UserName);
            if (family == null)
                return ResponseFamilyErrorEnum.UserNotFamilyOwner;
            else
            {
                family.Name = request.Name;
                var res = await _unitOfWork.Commit();
                return ResponseFamilyErrorEnum.Ok;
            }
        }
    }
}
