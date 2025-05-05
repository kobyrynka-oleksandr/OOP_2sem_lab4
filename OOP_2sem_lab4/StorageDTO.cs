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
    public class StorageDTO
    {
        public DbSet<Storage> Storages { get; set; }
        private const string ConnectionString = "Data Source=GreenSupply_DB.db";

        public List<Storage> GetListOfStoragesFromDB()
        {
            var storages = new List<Storage>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT Id, ServiceCost, Capacity FROM Storages", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var storage = new Storage
                    {
                        Id = reader.GetInt32(0),
                        ServiceCost = reader.GetDouble(1),
                        Capacity = reader.GetInt32(2)
                    };

                    ValidateStorageInput(storage);
                    storages.Add(storage);
                }
            }

            return storages;
        }

        public void AddStorage(Storage storage)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("INSERT INTO Storages (Id, ServiceCost, Capacity) VALUES (@id, @cost, @cap)", connection);
                command.Parameters.AddWithValue("@id", storage.Id);
                command.Parameters.AddWithValue("@cost", storage.ServiceCost);
                command.Parameters.AddWithValue("@cap", storage.Capacity);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteStorage(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var cmd1 = new SQLiteCommand("DELETE FROM ConsignmentsInStorage WHERE StorageId = @id", connection);
                cmd1.Parameters.AddWithValue("@id", id);
                cmd1.ExecuteNonQuery();

                var cmd2 = new SQLiteCommand("DELETE FROM Storages WHERE Id = @id", connection);
                cmd2.Parameters.AddWithValue("@id", id);
                cmd2.ExecuteNonQuery();
            }
        }

        public void AcceptConsignment(int storageId, Consignment consignment)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var select = new SQLiteCommand("SELECT Id FROM Consignments ORDER BY Id LIMIT 1", connection);
                var consignmentId = select.ExecuteScalar();
                if (consignmentId == null) return;

                string vegetableStats = consignment.Vegetable.ToString();

                var insert = new SQLiteCommand("INSERT INTO ConsignmentsInStorage (VegetableStats, Quantity, PricePerUnit, StorageId) VALUES (@vstats, @qn, @ppr, @sid)", connection);
                insert.Parameters.AddWithValue("@vstats", vegetableStats);
                insert.Parameters.AddWithValue("@qn", consignment.Quantity);
                insert.Parameters.AddWithValue("@ppr", consignment.PricePerUnit);
                insert.Parameters.AddWithValue("@sid", storageId);
                insert.ExecuteNonQuery();

                var delete = new SQLiteCommand("DELETE FROM Consignments WHERE Id = @cid", connection);
                delete.Parameters.AddWithValue("@cid", (int)(long)consignmentId);
                delete.ExecuteNonQuery();
            }
        }
        public void ValidateStorageInput(Storage storage)
        {
            string idText = storage.Id.ToString();
            string serviceCostText = storage.ServiceCost.ToString();
            string capacityText = storage.Capacity.ToString();

            var doubleRegex = new Regex(@"^\d+([.,]\d{1,2})?$");
            var idRegex = new Regex(@"^[1-9]\d*$");

            if (!idRegex.IsMatch(idText))
            {
                MessageBox.Show($"Некоректний номер складу: {idText}. Введіть додатне ціле число без ведучих нулів.");
                throw new Exception($"Некоректний номер складу: {idText}");
            }

            if (!doubleRegex.IsMatch(serviceCostText.Replace(',', '.')))
            {
                MessageBox.Show($"Некоректна вартість обслуговування: {serviceCostText}. Введіть додатне число до 2 знаків після крапки.");
                throw new Exception($"Некоректна вартість обслуговування: {serviceCostText}");
            }

            if (!doubleRegex.IsMatch(capacityText.Replace(',', '.')))
            {
                MessageBox.Show($"Некоректна вмістимість: {capacityText}. Введіть додатне число до 2 знаків після крапки.");
                throw new Exception($"Некоректна вмістимість: {capacityText}");
            }

            if (double.TryParse(serviceCostText.Replace(',', '.'), out double cost) && cost < 0)
            {
                MessageBox.Show("Вартість обслуговування має бути більшою за 0.");
                throw new Exception("Вартість обслуговування має бути більшою за 0.");
            }

            if (double.TryParse(capacityText.Replace(',', '.'), out double cap) && cap < 0)
            {
                MessageBox.Show("Вмістимість має бути більшою за 0.");
                throw new Exception("Вмістимість має бути більшою за 0.");
            }
        }
    }
}
