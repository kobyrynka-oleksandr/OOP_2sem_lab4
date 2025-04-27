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
            _vegetable.VegetableName = Name.Text;
            _vegetable.Country = Country.Text;
            _vegetable.NumOfSeason = int.Parse(NumOfSeason.Text);

            this.DialogResult = true;
            this.Close();
        }
    }
}
