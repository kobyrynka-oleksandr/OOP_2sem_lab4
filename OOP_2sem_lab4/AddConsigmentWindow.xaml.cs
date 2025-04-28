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
            if (VegetableComboBox.SelectedItem is Vegetable selectedVegetable &&
                TypeOfDelivComboBox.SelectedItem is DeliveryType selectedType)
            {
                var consignment = new Consignment
                {
                    VegetableName = selectedVegetable.VegetableName,
                    IdOfVegetable = selectedVegetable.Id,
                    Quantity = decimal.Parse(Quantity.Text),
                    PricePerUnit = decimal.Parse(PricePerUnit.Text),
                    CostOfDeliv = decimal.Parse(CostOfDeliv.Text),
                    TypeOfDeliv = selectedType.ToString(),
                    DelivDate = DelivDatePicker.SelectedDate?.ToString("yyyy-MM-dd") ?? DateTime.Now.ToString("yyyy-MM-dd")
                };

                NewConsignment = consignment;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Будь ласка, заповніть усі поля!");
            }
        }
    }
}
