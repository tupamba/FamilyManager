using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyManager.DataObject
{
    public class GroupFamily : IDisposable
    {
        public long Id { get; set; }
        [MaxLength(200)]
        [Index("IX_Name", 1, IsUnique = true)]
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<MemberFamily> MembersFamily { get; set; }
        public GroupFamily() { }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public GroupFamily(List<MemberFamily> members, string name)
        {
            MembersFamily = members;
            Name = name;
        }

        public void Dispose()
        {
        }
    }
    public class MemberFamily : IDisposable
    {
        public long Id { get; set; }
        [MaxLength(500)]
        [Index("IX_UserName", 1, IsUnique = true)]
        public string UserName { get; set; }
        public bool Owner { get; set; }
        public MemberFamily() { }
        public MemberFamily(string name, bool owner)
        {
            UserName = name;
            Owner = owner;
        }

        public void Dispose()
        {
        }
    }
}
