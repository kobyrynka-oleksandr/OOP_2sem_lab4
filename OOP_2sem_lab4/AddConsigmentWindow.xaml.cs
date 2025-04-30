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
    /// Interaction logic for AddConsigmentWindow.xaml
    /// </summary>
    public partial class AddConsigmentWindow : Window
    {
        public Consignment NewConsignment { get; set; }
        private VegetableDTO vegetableDB = new VegetableDTO();

        public AddConsigmentWindow()
        {
            InitializeComponent();

            VegetableComboBox.ItemsSource = vegetableDB.GetListFromDB();
            TypeOfDelivComboBox.ItemsSource = Enum.GetValues(typeof(DeliveryType)).Cast<DeliveryType>();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string quantityText = Quantity.Text.Trim();
            string pricePerUnitText = PricePerUnit.Text.Trim();
            string costOfDelivText = CostOfDeliv.Text.Trim();

            if (!IsValidInput(quantityText, pricePerUnitText, costOfDelivText))
                return;

            if (VegetableComboBox.SelectedItem is Vegetable selectedVegetable &&
                TypeOfDelivComboBox.SelectedItem is DeliveryType selectedType &&
                DelivDatePicker.SelectedDate != null)
            {
                var consignment = new Consignment
                {
                    VegetableId = selectedVegetable.Id,
                    Vegetable = selectedVegetable,               // навігаційна властивість (не обов'язково для збереження)
                    Quantity = decimal.Parse(quantityText),
                    PricePerUnit = decimal.Parse(pricePerUnitText),
                    CostOfDeliv = decimal.Parse(costOfDelivText),
                    TypeOfDeliv = selectedType.ToString(),
                    DelivDate = DelivDatePicker.SelectedDate?.ToString("yyyy-MM-dd") ?? DateTime.Now.ToString("yyyy-MM-dd")
                };

                NewConsignment = consignment;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Будь ласка, заповніть усі поля коректно!");
            }
        }

        private bool IsValidInput(string quantityText, string pricePerUnitText, string costOfDelivText)
        {
            var decimalRegex = new Regex(@"^\d+(\.\d{1,2})?$");

            if (!decimalRegex.IsMatch(quantityText))
            {
                MessageBox.Show("Некоректна кількість. Введіть число, наприклад: 10 або 10.5");
                return false;
            }

            if (!decimalRegex.IsMatch(pricePerUnitText))
            {
                MessageBox.Show("Некоректна ціна за одиницю. Введіть число, наприклад: 15 або 15.75");
                return false;
            }

            if (!decimalRegex.IsMatch(costOfDelivText))
            {
                MessageBox.Show("Некоректна вартість доставки. Введіть число, наприклад: 20 або 20.99");
                return false;
            }

            return true;
        }
    }
}
