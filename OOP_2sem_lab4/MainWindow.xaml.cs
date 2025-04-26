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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OOP_2sem_lab4
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainGrid.Visibility = Visibility.Visible;
            GardenGrid.Visibility = Visibility.Collapsed;
            ConsignmentGrid.Visibility = Visibility.Collapsed;
            StorageGrid.Visibility = Visibility.Collapsed;
        }

        private void Button_Window_Main_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Visible;
            GardenGrid.Visibility = Visibility.Collapsed;
            ConsignmentGrid.Visibility = Visibility.Collapsed;
            StorageGrid.Visibility = Visibility.Collapsed;
        }

        private void Button_Window_Garden_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Collapsed;
            GardenGrid.Visibility = Visibility.Visible;
            ConsignmentGrid.Visibility = Visibility.Collapsed;
            StorageGrid.Visibility = Visibility.Collapsed;
        }

        private void Button_Window_Сonsignment_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Collapsed;
            GardenGrid.Visibility = Visibility.Collapsed;
            ConsignmentGrid.Visibility = Visibility.Visible;
            StorageGrid.Visibility = Visibility.Collapsed;
        }

        private void Button_Window_Storage_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Collapsed;
            GardenGrid.Visibility = Visibility.Collapsed;
            ConsignmentGrid.Visibility = Visibility.Collapsed;
            StorageGrid.Visibility = Visibility.Visible;
        }
    }
}
