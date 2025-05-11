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
using System.ComponentModel.DataAnnotations;

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

            ClearHighlight(Name, Country, NumOfSeason);

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
            bool inputValid = true;

            if (!int.TryParse(numOfSeasonText, out int season))
            {
                HighlightInvalid(NumOfSeason);
                MessageBox.Show("Сезон має бути числом від 1 до 4.");
                return false;
            }

            var vegetable = new Vegetable(name, country, season);
            var context = new ValidationContext(vegetable, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            bool isValid = Validator.TryValidateObject(vegetable, context, results, true);

            if (!isValid)
            {
                foreach (var result in results)
                {
                    if (result.MemberNames.Contains(nameof(Vegetable.VegetableName)))
                        HighlightInvalid(Name);

                    if (result.MemberNames.Contains(nameof(Vegetable.Country)))
                        HighlightInvalid(Country);

                    if (result.MemberNames.Contains(nameof(Vegetable.NumOfSeason)))
                        HighlightInvalid(NumOfSeason);
                }

                string message = string.Join("\n", results.Select(r => r.ErrorMessage));
                MessageBox.Show(message, "Помилки валідації.");
                inputValid = false;
            }

            return inputValid;
        }

        private void HighlightInvalid(Control control)
        {
            control.BorderBrush = Brushes.Red;
            control.BorderThickness = new Thickness(2);
        }

        private void ClearHighlight(params Control[] controls)
        {
            foreach (var control in controls)
            {
                control.BorderBrush = Brushes.Gray;
                control.BorderThickness = new Thickness(1);
            }
        }
    }
}
