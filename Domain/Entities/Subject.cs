using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Subject : BaseEntity
{
    public string NameSubject { get; set; }

    public ICollection<Notes> Notes { get; set; }
}