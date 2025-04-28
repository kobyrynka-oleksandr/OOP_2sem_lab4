using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_2sem_lab4
{
    public class Vegetable
    {
        public int Id { get; set; }
        public string VegetableName { get; set; }
        public string Country { get; set; }
        public int NumOfSeason { get; set; }

        public Vegetable() { }
        
        public Vegetable(string VegetableName, string Country, int NumOfSeason)
        {
            this.VegetableName = VegetableName;
            this.Country = Country;
            this.NumOfSeason = NumOfSeason;
        }
        public override string ToString()
        {
            return $"{VegetableName} ({Country}, сезон {NumOfSeason})";
        }
    }
}
