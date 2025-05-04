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
    /// Interaction logic for AddStorageWindow.xaml
    /// </summary>
    public partial class AddStorageWindow : Window
    {
        public Storage NewStorage { get; private set; }

        public AddStorageWindow()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string idText = Id.Text.Trim();
            string serviceCostText = ServiceCost.Text.Trim();
            string capacityText = Capacity.Text.Trim();

            if (!IsValidInput(idText, serviceCostText, capacityText))
                return;

            int id = int.Parse(idText);
            double serviceCost = double.Parse(serviceCostText.Replace(',', '.'));
            int capacity = int.Parse(capacityText.Replace(',', '.'));

            NewStorage = new Storage(id, serviceCost, capacity);
            this.DialogResult = true;
            this.Close();
        }

        private bool IsValidInput(string idText, string serviceCostText, string capacityText)
        {
            var doubleRegex = new Regex(@"^\d+([.,]\d{1,2})?$");
            var idRegex = new Regex(@"^[1-9]\d*$"); // Додатне ціле число без ведучих нулів

            if (!idRegex.IsMatch(idText))
            {
                MessageBox.Show("Некоректний номер складу. Введіть додатне ціле число без ведучих нулів.");
                return false;
            }

            if (!doubleRegex.IsMatch(serviceCostText))
            {
                MessageBox.Show("Некоректна вартість обслуговування. Введіть додатне число (до 2 знаків після крапки).");
                return false;
            }

            if (!doubleRegex.IsMatch(capacityText))
            {
                MessageBox.Show("Некоректна вмістимість. Введіть додатне число (до 2 знаків після крапки).");
                return false;
            }

            if (double.TryParse(serviceCostText.Replace(',', '.'), out double cost) && cost <= 0)
            {
                MessageBox.Show("Вартість має бути більшою за 0.");
                return false;
            }

            if (double.TryParse(capacityText.Replace(',', '.'), out double cap) && cap <= 0)
            {
                MessageBox.Show("Вмістимість має бути більшою за 0.");
                return false;
            }

            return true;
        }
    }
}
