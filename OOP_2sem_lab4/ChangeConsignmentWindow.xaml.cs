using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OOP_2sem_lab4
{
    /// <summary>
    /// Interaction logic for ChangeConsignmentWindow.xaml
    /// </summary>
    public partial class ChangeConsignmentWindow : Window
    {
        private Consignment _consignment;
        private VegetableDTO vegetableDB = new VegetableDTO();

        public ChangeConsignmentWindow(Consignment consignment)
        {
            InitializeComponent();
            _consignment = consignment;

            var vegetables = vegetableDB.GetListFromDB();
            VegetableComboBox.ItemsSource = vegetables;
            VegetableComboBox.SelectedItem = vegetables.FirstOrDefault(v => v.Id == consignment.VegetableId);

            TypeOfDelivComboBox.ItemsSource = Enum.GetValues(typeof(DeliveryType)).Cast<DeliveryType>();
            if (Enum.TryParse(_consignment.TypeOfDeliv, out DeliveryType parsedType))
            {
                TypeOfDelivComboBox.SelectedItem = parsedType;
            }

            Quantity.Text = _consignment.Quantity.ToString();
            PricePerUnit.Text = _consignment.PricePerUnit.ToString();
            CostOfDeliv.Text = _consignment.CostOfDeliv.ToString();
            if (DateTime.TryParse(_consignment.DelivDate, out DateTime parsedDate))
                DelivDatePicker.SelectedDate = parsedDate;
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            string quantityText = Quantity.Text.Trim();
            string priceText = PricePerUnit.Text.Trim();
            string costText = CostOfDeliv.Text.Trim();

            if (!IsValidInput(quantityText, priceText, costText))
                return;

            if (VegetableComboBox.SelectedItem is Vegetable selectedVeg &&
                TypeOfDelivComboBox.SelectedItem is DeliveryType selectedType &&
                DelivDatePicker.SelectedDate is DateTime selectedDate)
            {
                _consignment.Vegetable = selectedVeg;
                _consignment.Quantity = decimal.Parse(quantityText);
                _consignment.PricePerUnit = decimal.Parse(priceText);
                _consignment.CostOfDeliv = decimal.Parse(costText);
                _consignment.TypeOfDeliv = selectedType.ToString();
                _consignment.DelivDate = selectedDate.ToString("yyyy-MM-dd");

                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Будь ласка, заповніть усі поля коректно.");
            }
        }

        private bool IsValidInput(string quantity, string price, string cost)
        {
            var decimalRegex = new Regex(@"^\d+(\.\d{1,2})?$");

            if (!decimalRegex.IsMatch(quantity))
            {
                MessageBox.Show("Некоректна кількість. Введіть число, наприклад 5 або 5.25");
                return false;
            }

            if (!decimalRegex.IsMatch(price))
            {
                MessageBox.Show("Некоректна ціна за одиницю. Введіть число, наприклад 3 або 3.50");
                return false;
            }

            if (!decimalRegex.IsMatch(cost))
            {
                MessageBox.Show("Некоректна вартість доставки. Введіть число, наприклад 10 або 10.75");
                return false;
            }

            return true;
        }
    }
}
