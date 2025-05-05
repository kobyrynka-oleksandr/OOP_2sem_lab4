using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SQLite;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Windows;

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
            List<Vegetable> listOfVegetables = Vegetables.ToList();
            foreach (var vegetable in Vegetables)
                IsValidInput(vegetable);
            return listOfVegetables;
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
        private void IsValidInput(Vegetable vegetable)
        {
            string name = vegetable.VegetableName;
            string country = vegetable.Country;
            string numOfSeasonText = vegetable.NumOfSeason.ToString();

            var nameRegex = new Regex(@"^[А-Яа-яЇїІіЄєҐґA-Za-z\s]+$");
            var countryRegex = new Regex(@"^[А-Яа-яЇїІіЄєҐґA-Za-z\s]+$");
            var seasonRegex = new Regex(@"^[1-4]$");

            if (!nameRegex.IsMatch(name))
            {
                int id = vegetable.Id;
                MessageBox.Show($"Назва городини має не правильний формат! Можливе id городини {id}");
                throw new Exception($"Назва городини має не правильний формат! Можливе id городини {id}");
            }

            if (!countryRegex.IsMatch(country))
            {
                int id = vegetable.Id;
                MessageBox.Show($"Країна походження має не правильний формат! Можливе id городини {id}");
                throw new Exception($"Країна походження має не правильний формат! Можливе id городини {id}");
            }

            if (!seasonRegex.IsMatch(numOfSeasonText))
            {
                int id = vegetable.Id;
                MessageBox.Show($"Номер сезону визрівання має не правильний формат! Можливе id городини {id}");
                throw new Exception($"Номер сезону визрівання має не правильний формат! Можливе id городини {id}");
            }
        }
    }
}
