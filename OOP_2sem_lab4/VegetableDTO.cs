using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace OOP_2sem_lab4
{
    public class VegetableDTO : DbContext
    {
        public DbSet<Vegetable> Vegetables { get; set; }

        public VegetableDTO() : base("DefaultConnection") { }
    }
}
