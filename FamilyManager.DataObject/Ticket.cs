using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyManager.DataObject
{
    public class Ticket
    {
        public long Id { get; set; }
        public DateTime Create { get; set; }
        public DateTime LastModify { get; set; }
        public MemberFamily UserOwner { get; set; }
        public TicketSate State { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
    public enum TicketSate
    {
        todo,
        closed,
        buyed
    }
}
