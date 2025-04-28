using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_2sem_lab4
{
    public class Consignment
    {
        public int Id { get; set; }
        public string VegetableName { get; set; }
        public int IdOfVegetable { get; set; }
        public decimal Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal CostOfDeliv { get; set; }
        public string TypeOfDeliv { get; set; }
        public string DelivDate { get; set; }

        public Consignment() { }

        public Consignment(string vegetableName, int idOfVegetable, decimal quantity, decimal pricePerUnit, decimal costOfDeliv, string typeOfDeliv, string delivDate)
        {
            VegetableName = vegetableName;
            IdOfVegetable = idOfVegetable;
            Quantity = quantity;
            PricePerUnit = pricePerUnit;
            CostOfDeliv = costOfDeliv;
            TypeOfDeliv = typeOfDeliv;
            DelivDate = delivDate;
        }
    }
}
