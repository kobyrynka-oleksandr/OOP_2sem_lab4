using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OOP_2sem_lab4
{
    /// <summary>
    /// Interaction logic for AddVegetableWindow.xaml
    /// </summary>
    public partial class AddVegetableWindow : Window
    {
        public Vegetable NewVegetable { get; private set; }
        public AddVegetableWindow()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string name = Name.Text;
            string country = Country.Text;
            int numOfSeason = int.Parse(NumOfSeason.Text);
            NewVegetable = new Vegetable(name, country, numOfSeason);
            this.DialogResult = true;
            this.Close();
        }
    }
}
