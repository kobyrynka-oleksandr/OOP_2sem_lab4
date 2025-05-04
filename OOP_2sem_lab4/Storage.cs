using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_2sem_lab4
{
    public class Storage
    {
        public int Id { get; set; } 
        public double ServiceCost { get; set; }
        public int Capacity { get; set; }

        public Storage() { }

        public Storage(int id, double serviceCost, int capacity)
        {
            Id = id;
            ServiceCost = serviceCost;
            Capacity = capacity;
        }

        public Storage(double serviceCost, int capacity)
        {
            ServiceCost = serviceCost;
            Capacity = capacity;
        }

        public override string ToString()
        {
            return $"Склад №{Id} (місткість: {Capacity}, обслуговування: {ServiceCost} грн)";
        }

        public string ToShortString(decimal totalValue)
        {
            return $"Склад №{Id}: загальна вартість товарів = {totalValue} грн";
        }
    }
}
