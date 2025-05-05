using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace OOP_2sem_lab4
{
    public class ConsignmentDTO : DbContext
    {
        public DbSet<Consignment> Consignments { get; set; }
        public DbSet<Vegetable> Vegetables { get; set; }

        public ConsignmentDTO() : base("DefaultConnection") { }

        public void SaveToDB(List<Consignment> consignmentList)
        {
            foreach (var consignment in consignmentList)
            {
                var existingVegetable = Vegetables.FirstOrDefault(v => v.Id == consignment.VegetableId);

                if (existingVegetable == null)
                {
                    Vegetables.Add(consignment.Vegetable);
                }
                else
                {
                    consignment.Vegetable = existingVegetable;
                }

                Consignments.Add(consignment);
            }

            SaveChanges();
            consignmentList.Clear();
        }
        public List<Consignment> GetListFromDB()
        {
            var list = Consignments.Include(c => c.Vegetable).ToList();

            foreach (var item in list)
            {
                IsValidInput(item);
            }

            return list;
        }

        public static void UpdateConsignment(Consignment consignment)
        {
            using (var connection = new SQLiteConnection("Data Source=GreenSupply_DB.db"))
            {
                connection.Open();

                string query = @"UPDATE Consignments 
                                 SET VegetableId = @VegetableId,
                                     Quantity = @Quantity, 
                                     PricePerUnit = @PricePerUnit, 
                                     CostOfDeliv = @CostOfDeliv, 
                                     TypeOfDeliv = @TypeOfDeliv, 
                                     DelivDate = @DelivDate
                                 WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@VegetableId", consignment.VegetableId);
                    command.Parameters.AddWithValue("@Quantity", consignment.Quantity);
                    command.Parameters.AddWithValue("@PricePerUnit", consignment.PricePerUnit);
                    command.Parameters.AddWithValue("@CostOfDeliv", consignment.CostOfDeliv);
                    command.Parameters.AddWithValue("@TypeOfDeliv", consignment.TypeOfDeliv);
                    command.Parameters.AddWithValue("@DelivDate", consignment.DelivDate);
                    command.Parameters.AddWithValue("@Id", consignment.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteConsignment(Consignment consignment)
        {
            using (var connection = new SQLiteConnection("Data Source=GreenSupply_DB.db"))
            {
                connection.Open();

                string query = "DELETE FROM Consignments WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", consignment.Id);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void IsValidInput(Consignment consignment)
        {
            string quantityText = consignment.Quantity.ToString();
            string pricePerUnitText = consignment.PricePerUnit.ToString();
            string costOfDelivText = consignment.CostOfDeliv.ToString();
            string delivDate = consignment.DelivDate;
            string typeOfDeliv = consignment.TypeOfDeliv;

            var decimalRegex = new Regex(@"^\d+(\.\d{1,2})?$");
            var dateRegex = new Regex(@"^\d{4}-\d{2}-\d{2}$");
            var typeRegex = new Regex(@"^[А-Яа-яЇїІіЄєҐґA-Za-z\s]+$");

            if (!decimalRegex.IsMatch(quantityText))
            {
                int id = consignment.Id;
                MessageBox.Show($"Кількість має некоректний формат! Можливе id партії: {id}");
                throw new Exception($"Кількість має некоректний формат! Можливе id партії: {id}");
            }

            if (!decimalRegex.IsMatch(pricePerUnitText))
            {
                int id = consignment.Id;
                MessageBox.Show($"Ціна за одиницю має некоректний формат! Можливе id партії: {id}");
                throw new Exception($"Ціна за одиницю має некоректний формат! Можливе id партії: {id}");
            }

            if (!decimalRegex.IsMatch(costOfDelivText))
            {
                int id = consignment.Id;
                MessageBox.Show($"Вартість доставки має некоректний формат! Можливе id партії: {id}");
                throw new Exception($"Вартість доставки має некоректний формат! Можливе id партії: {id}");
            }

            if (!typeRegex.IsMatch(typeOfDeliv))
            {
                int id = consignment.Id;
                MessageBox.Show($"Тип доставки має некоректний формат! Можливе id партії: {id}");
                throw new Exception($"Тип доставки має некоректний формат! Можливе id партії: {id}");
            }

            if (!dateRegex.IsMatch(delivDate))
            {
                int id = consignment.Id;
                MessageBox.Show($"Дата доставки має бути у форматі yyyy-MM-dd! Можливе id партії: {id}");
                throw new Exception($"Дата доставки має бути у форматі yyyy-MM-dd! Можливе id партії: {id}");
            }

            if (!DateTime.TryParseExact(delivDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
            {
                int id = consignment.Id;
                MessageBox.Show($"Дата доставки є недійсною або має неправильний формат! Можливе id партії: {id}");
                throw new Exception($"Дата доставки є недійсною або має неправильний формат! Можливе id партії: {id}");
            }
        }
    }
}
