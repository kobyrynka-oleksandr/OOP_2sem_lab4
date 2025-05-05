using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

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
                        var consignment = new ConsignmentInStorage
                        {
                            Id = reader.GetInt32(0),
                            VegetableStats = reader.GetString(1),
                            Quantity = reader.GetDouble(2),
                            PricePerUnit = reader.GetDouble(3),
                            StorageId = currentStorageId
                        };
                        IsValidInput(consignment);
                        consignments.Add(consignment);
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
        private void IsValidInput(ConsignmentInStorage consignmentInStorage)
        {
            string vegetableStats = consignmentInStorage.VegetableStats;
            string quantityText = consignmentInStorage.Quantity.ToString();
            string pricePerUnitText = consignmentInStorage.PricePerUnit.ToString();

            var decimalRegex = new Regex(@"^\d+(\.\d{1,2})?$");
            var vegetableStatsRegex = new Regex(@"^[\p{L}\s]+ \([\p{L}\s]+, сезон \d+\)$");

            int id = consignmentInStorage.Id;

            if (!decimalRegex.IsMatch(quantityText))
            {
                MessageBox.Show($"Кількість має некоректний формат! Можливе id партії: {id}");
                throw new Exception($"Кількість має некоректний формат! Можливе id партії: {id}");
            }

            if (!decimalRegex.IsMatch(pricePerUnitText))
            {
                MessageBox.Show($"Ціна за одиницю має некоректний формат! Можливе id партії: {id}");
                throw new Exception($"Ціна за одиницю має некоректний формат! Можливе id партії: {id}");
            }

            if (!vegetableStatsRegex.IsMatch(vegetableStats))
            {
                MessageBox.Show($"Формат VegetableStats неправильний! Має бути: 'Назва (Країна, сезон N)'. Можливе id партії: {id}");
                throw new Exception($"Формат VegetableStats неправильний! Можливе id партії: {id}");
            }
        }
    }
}
