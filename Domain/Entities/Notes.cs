using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Notes : BaseEntity
    {
        public int IdSubject { get; set; }
        public Subject Subject { get; set; }
        public int IdStudent { get; set; }
        public Student Student{ get; set; }
        public double Note1 { get; set; }
        public double Note2 { get; set; }
        public double Note3 { get; set; }
        public double Average { get; set; }
    }
}
    
