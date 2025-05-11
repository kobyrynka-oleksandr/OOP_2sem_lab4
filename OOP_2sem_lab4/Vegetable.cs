using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OOP_2sem_lab4
{
    public class Vegetable
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва городини обов'язкова.")]
        [RegularExpression(@"^[А-Яа-яЇїІіЄєҐґA-Za-z\s]+$", ErrorMessage = "Назва городини має неправильний формат.")]
        public string VegetableName { get; set; }

        [Required(ErrorMessage = "Країна походження обов'язкова")]
        [RegularExpression(@"^[А-Яа-яЇїІіЄєҐґA-Za-z\s]+$", ErrorMessage = "Країна походження має неправильний формат.")]
        public string Country { get; set; }

        [Range(1, 4, ErrorMessage = "Номер сезону повинен бути в межах 1-4.")]
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
