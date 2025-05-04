using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_2sem_lab4
{
    public class ConsignmentInStorageDTO
    {
        public DbSet<ConsignmentInStorage> ConsignmentsInStorage { get; set; }
        private const string ConnectionString = "Data Source=GreenSupply_DB.db";

        public List<ConsignmentInStorage> GetListOfConsignmentsInStoragesFromDB(int storageId)
        {
            var consignments = new List<ConsignmentInStorage>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT Id, VegetableStats, Quantity, PricePerUnit, StorageId FROM ConsignmentsInStorage", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int currentStorageId = reader.GetInt32(4);
                    if (currentStorageId == storageId)
                    {
                        consignments.Add(new ConsignmentInStorage
                        {
                            Id = reader.GetInt32(0),
                            VegetableStats = reader.GetString(1),
                            Quantity = reader.GetDouble(2),
                            PricePerUnit = reader.GetDouble(3),
                            StorageId = currentStorageId
                        });
                    }
                }
            }
            return consignments;
        }
        public void RejectConsignment(int consignmentId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = new SQLiteCommand("DELETE FROM ConsignmentsInStorage WHERE Id = @id", connection);
                cmd.Parameters.AddWithValue("@id", consignmentId);
                cmd.ExecuteNonQuery();
            }
        }
        public double GetTotalQuantityForStorage(int storageId)
        {
            var consignments = GetListOfConsignmentsInStoragesFromDB(storageId);
            return consignments.Sum(c => c.Quantity);
        }
        public double GetTotalCostForStorage(int storageId)
        {
            var consignments = GetListOfConsignmentsInStoragesFromDB(storageId);
            return consignments.Sum(c => c.Quantity * c.PricePerUnit);
        }
        public string ToShortString(int storageId)
        {
            double totalWeight = GetTotalQuantityForStorage(storageId);
            double totalCost = GetTotalCostForStorage(storageId);
            return $"Номер складу: {storageId}, загальна вага товарів: {totalWeight} кг, загальна вартість: {totalCost} грн";
        }
    }
}
