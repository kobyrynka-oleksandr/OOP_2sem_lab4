using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SQLite;
using System.Xml.Linq;

namespace OOP_2sem_lab4
{
    public class VegetableDTO : DbContext
    {
        public DbSet<Vegetable> Vegetables { get; set; }

        public VegetableDTO() : base("DefaultConnection") { }

        public void SaveToDB(List<Vegetable> vegetableList)
        {
            Vegetables.AddRange(vegetableList);
            SaveChanges();
            vegetableList.Clear();
        }
        public List<Vegetable> GetListFromDB()
        {
            return Vegetables.ToList();
        }
        public static void UpdateVegetable(Vegetable vegetable)
        {
            using (var connection = new SQLiteConnection("Data Source=GreenSupply_DB.db"))
            {
                connection.Open();

                string query = "UPDATE Vegetables SET VegetableName = @VegetableName, Country = @Country, NumOfSeason = @NumOfSeason WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@VegetableName", vegetable.VegetableName);
                    command.Parameters.AddWithValue("@Country", vegetable.Country);
                    command.Parameters.AddWithValue("@NumOfSeason", vegetable.NumOfSeason);
                    command.Parameters.AddWithValue("@Id", vegetable.Id);

                    command.ExecuteNonQuery();
                }
            }
        }
        public static void DeleteVegetable(Vegetable vegetable)
        {
            using (var connection = new SQLiteConnection("Data Source=GreenSupply_DB.db"))
            {
                connection.Open();

                using (var pragmaCmd = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
                {
                    pragmaCmd.ExecuteNonQuery();
                }

                string query = "DELETE FROM Vegetables WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", vegetable.Id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
