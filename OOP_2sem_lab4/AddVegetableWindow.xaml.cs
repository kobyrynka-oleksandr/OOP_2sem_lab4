using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OOP_2sem_lab4
{
    public partial class AddVegetableWindow : Window
    {
        public Vegetable NewVegetable { get; private set; }

        public AddVegetableWindow()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string name = Name.Text.Trim();
            string country = Country.Text.Trim();
            string numOfSeasonText = NumOfSeason.Text.Trim();

            ClearHighlight(Name, Country, NumOfSeason);

            if (!IsValidInput(name, country, numOfSeasonText))
            {
                return;
            }

            int numOfSeason = int.Parse(numOfSeasonText);
            NewVegetable = new Vegetable(name, country, numOfSeason);

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
