using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_2sem_lab4
{
    public class ConsignmentDTO : DbContext
    {
        public DbSet<Consignment> Consignments { get; set; }

        public ConsignmentDTO() : base("DefaultConnection") { }

        public void SaveToDB(List<Consignment> consignmentList)
        {
            Consignments.AddRange(consignmentList);
            SaveChanges();
            consignmentList.Clear();
        }

        public List<Consignment> GetListFromDB()
        {
            return Consignments.ToList();
        }

        public static void UpdateConsignment(Consignment consignment)
        {
            using (var connection = new SQLiteConnection("Data Source=GreenSupply_DB.db"))
            {
                connection.Open();

                string query = @"UPDATE Consignments 
                             SET VegetableName = @VegetableName,
                                 IdOfVegetable = @IdOfVegetable
                                 Quantity = @Quantity, 
                                 PricePerUnit = @PricePerUnit, 
                                 CostOfDeliv = @CostOfDeliv, 
                                 TypeOfDeliv = @TypeOfDeliv, 
                                 DelivDate = @DelivDate 
                             WHERE Id = @Id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@VegetableName", consignment.VegetableName);
                    command.Parameters.AddWithValue("@IdOfVegetable", consignment.IdOfVegetable);
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
    }
}
