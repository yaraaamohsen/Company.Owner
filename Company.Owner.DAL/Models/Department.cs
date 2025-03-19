﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Owner.DAL.Models
{
    public class Department : BaseEntity
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public DateTime CreateAt { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
