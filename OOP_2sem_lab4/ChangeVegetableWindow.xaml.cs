using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace OOP_2sem_lab4
{
    /// <summary>
    /// Interaction logic for ChangeVegetableWindow.xaml
    /// </summary>
    public partial class ChangeVegetableWindow : Window
    {
        private Vegetable _vegetable;
        public ChangeVegetableWindow(Vegetable vegetable)
        {
            InitializeComponent();
            _vegetable = vegetable;

            Name.Text = vegetable.VegetableName;
            Country.Text = vegetable.Country;
            NumOfSeason.Text = vegetable.NumOfSeason.ToString();
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            string name = Name.Text.Trim();
            string country = Country.Text.Trim();
            string numOfSeasonText = NumOfSeason.Text.Trim();

            if (!IsValidInput(name, country, numOfSeasonText))
            {
                return;
            }

            _vegetable.VegetableName = name;
            _vegetable.Country = country;
            _vegetable.NumOfSeason = int.Parse(numOfSeasonText);

            this.DialogResult = true;
            this.Close();
        }

        private bool IsValidInput(string name, string country, string numOfSeasonText)
        {
            var nameRegex = new Regex(@"^[А-Яа-яЇїІіЄєҐґA-Za-z\s]+$");
            var countryRegex = new Regex(@"^[А-Яа-яЇїІіЄєҐґA-Za-z\s]+$");
            var seasonRegex = new Regex(@"^[1-4]$"); // тільки 1, 2, 3 або 4

            if (!nameRegex.IsMatch(name))
            {
                MessageBox.Show("Некоректна назва овоча. Використовуйте тільки букви та пробіли.");
                return false;
            }

            if (!countryRegex.IsMatch(country))
            {
                MessageBox.Show("Некоректна країна походження. Використовуйте тільки букви та пробіли.");
                return false;
            }

            if (!seasonRegex.IsMatch(numOfSeasonText))
            {
                MessageBox.Show("Некоректний номер сезону. Введіть число від 1 до 4.");
                return false;
            }

            return true;
        }
    }
}
