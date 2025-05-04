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
        public int VegetableId { get; set; }
        public virtual Vegetable Vegetable { get; set; }
        public decimal Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal CostOfDeliv { get; set; }
        public string TypeOfDeliv { get; set; }
        public string DelivDate { get; set; }

        public Consignment() { }

        public Consignment(int vegetableId, Vegetable vegetable, decimal quantity, decimal pricePerUnit, decimal costOfDeliv, string typeOfDeliv, string delivDate)
        {
            VegetableId = vegetableId;
            Vegetable = vegetable;
            Quantity = quantity;
            PricePerUnit = pricePerUnit;
            CostOfDeliv = costOfDeliv;
            TypeOfDeliv = typeOfDeliv;
            DelivDate = delivDate;
        }
        public override string ToString()
        {
            return $"Городина: {Vegetable.ToString()}, кількість: {Quantity}, ціна за одиницю: {PricePerUnit}, вартість тр-ння: {CostOfDeliv}, тип доставки: {TypeOfDeliv}, дата поставки: {DelivDate}";
        }
    }
}
