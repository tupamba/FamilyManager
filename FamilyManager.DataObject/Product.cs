using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyManager.DataObject
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public CategoryProduct CategoryProduct { get; set; }
        public virtual GroupFamily GroupFamily { get; set; }
        public virtual ICollection<Ticket> Ticket { get; set; }
    }
    public class CategoryProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual FamilyCategory FamilyCategory { get; set; }
    }
    public class FamilyCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
