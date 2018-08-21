using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FamilyManager.WebApi.Command.FamilyController;
using System.Threading;
using FamilyManager.WebApi.Models;
using System.Threading.Tasks;
using FamilyManager.DataObject;
using FamilyManager.Repository;
using System.Collections.Generic;
using System.Data.Entity;

namespace FamilyManager.WebApiTests.Command
{
    [TestClass]
    public class CommandTest
    {
        [TestMethod()]
        public void TestCommandAddFamilyGroup()
        {
            var mockSet = new Mock<DbSet<GroupFamily>>();
            var mockContext = new Mock<FamilyManager.DataProvider.DbModel>();
            mockContext.Setup(c => c.GroupFamily).Returns(mockSet.Object);

            var family = new GroupFamily
            {
                Name = "Prueba",
                MembersFamily = new List<MemberFamily>
                {
                     new MemberFamily { UserName = "Prueba" }
                }
            };
            var repository = new Mock<IGroupFamilyRepository>();
            var mockUnitWork = new Mock<IUnitOfWork>();
            repository
    .Setup(m => m.AddGroupFamily(family))
    .Returns(Task.FromResult<GroupFamily>(family));
            mockUnitWork
    .Setup(m => m.Commit())
    .Returns(Task.FromResult<int>(1));

            var command = new AddGroupFamilyCommand(new GroupFamily()
            { Name = "Nuevo" }, "pepe");
            var handler = new AddGroupFamilyCommandHandler(repository.Object, mockUnitWork.Object);
            var r = handler.Handle(command, new CancellationToken());
            r.Wait();
            var result = r.Result;
            Assert.IsTrue(result == ResponseFamilyErrorEnum.Ok);
        }
    }
}
