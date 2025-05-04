using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_2sem_lab4
{
    public class ConsignmentInStorage
    {
        public int Id { get; set; }
        public string VegetableStats { get; set; }
        public double Quantity { get; set; }
        public double PricePerUnit { get; set; }
        public int StorageId { get; set; }

        public ConsignmentInStorage() { }

        public ConsignmentInStorage(int id, string vegetableStats, double quantity, double pricePerUnit, int storageId)
        {
            Id = id;
            VegetableStats = vegetableStats;
            Quantity = quantity;
            PricePerUnit = pricePerUnit;
            StorageId = storageId;
        }

        public override string ToString()
        {
            return $"{VegetableStats} - {Quantity} од. по {PricePerUnit} грн (Склад #{StorageId})";
        }
    }
}
