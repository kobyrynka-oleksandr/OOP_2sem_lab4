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
using System.Xml.Linq;

namespace OOP_2sem_lab4
{
    public partial class MainWindow : Window
    {
        List<Vegetable> vegetableList = new List<Vegetable>();
        VegetableDTO vegetableDB;
        List<Consignment> consignmentList = new List<Consignment>();
        ConsignmentDTO consignmentDB;
        public MainWindow()
        {
            InitializeComponent();

            MainGrid.Visibility = Visibility.Visible;
            GardenGrid.Visibility = Visibility.Collapsed;
            ConsignmentGrid.Visibility = Visibility.Collapsed;
            StorageGrid.Visibility = Visibility.Collapsed;

            vegetableDB = new VegetableDTO();
            consignmentDB = new ConsignmentDTO();

            List<Vegetable> garden = vegetableDB.Vegetables.ToList();
            string strGarden = "";
            foreach (var vegetable in garden)
                strGarden += "Name: " + vegetable.VegetableName + " " + "Country: " + vegetable.Country + " " + "Season: " + vegetable.NumOfSeason + "\n";

            gardenText.Text = strGarden;
        }

        private void Button_Window_Main_Click(object sender, RoutedEventArgs e)
        {
            if (vegetableList.Count != 0)
            {
                ConfirmChangesToDB form = new ConfirmChangesToDB();

                if (form.ShowDialog() == true)
                    SaveVegetablesToDatabase();
                else
                {
                    vegetableList.Clear();
                    RefreshGardenData();
                }
            }

            if (consignmentList.Count != 0)
            {
                ConfirmChangesToDB form = new ConfirmChangesToDB();

                if (form.ShowDialog() == true)
                    SaveConsignmentsToDatabase();
                else
                {
                    consignmentList.Clear();
                    RefreshConsignmentData();
                }
            }

            MainGrid.Visibility = Visibility.Visible;
            GardenGrid.Visibility = Visibility.Collapsed;
            ConsignmentGrid.Visibility = Visibility.Collapsed;
            StorageGrid.Visibility = Visibility.Collapsed;
        }

        private void Button_Window_Garden_Click(object sender, RoutedEventArgs e)
        {
            RefreshGardenData();
            if (consignmentList.Count != 0)
            {
                ConfirmChangesToDB form = new ConfirmChangesToDB();

                if (form.ShowDialog() == true)
                    SaveConsignmentsToDatabase();
                else
                {
                    consignmentList.Clear();
                    RefreshConsignmentData();
                }
            }
            MainGrid.Visibility = Visibility.Collapsed;
            GardenGrid.Visibility = Visibility.Visible;
            ConsignmentGrid.Visibility = Visibility.Collapsed;
            StorageGrid.Visibility = Visibility.Collapsed;
        }

        private void Button_Window_Сonsignment_Click(object sender, RoutedEventArgs e)
        {
            RefreshConsignmentData();
            if (vegetableList.Count != 0)
            {
                ConfirmChangesToDB form = new ConfirmChangesToDB();

                if (form.ShowDialog() == true)
                    SaveVegetablesToDatabase();
                else
                {
                    vegetableList.Clear();
                    RefreshGardenData();
                }
            }

            MainGrid.Visibility = Visibility.Collapsed;
            GardenGrid.Visibility = Visibility.Collapsed;
            ConsignmentGrid.Visibility = Visibility.Visible;
            StorageGrid.Visibility = Visibility.Collapsed;
        }

        private void Button_Window_Storage_Click(object sender, RoutedEventArgs e)
        {
            if (vegetableList.Count != 0)
            {
                ConfirmChangesToDB form = new ConfirmChangesToDB();

                if (form.ShowDialog() == true)
                    SaveVegetablesToDatabase();
                else
                {
                    vegetableList.Clear();
                    RefreshGardenData();
                }
            }

            if (consignmentList.Count != 0)
            {
                ConfirmChangesToDB form = new ConfirmChangesToDB();

                if (form.ShowDialog() == true)
                    SaveConsignmentsToDatabase();
                else
                {
                    consignmentList.Clear();
                    RefreshConsignmentData();
                }
            }

            MainGrid.Visibility = Visibility.Collapsed;
            GardenGrid.Visibility = Visibility.Collapsed;
            ConsignmentGrid.Visibility = Visibility.Collapsed;
            StorageGrid.Visibility = Visibility.Visible;
        }

        private void Add_Vegetable_Click(object sender, RoutedEventArgs e)
        {
            AddVegetableWindow addVegetableWindow = new AddVegetableWindow();
            if (addVegetableWindow.ShowDialog() == true)
            {
                vegetableList.Add(addVegetableWindow.NewVegetable);
                RefreshGardenData();
            }
        }
        private void Change_Vegetable_Click(object sender, RoutedEventArgs e)
        {
            var selectedVegetable = GardenData.SelectedItem as Vegetable;

            if (selectedVegetable == null)
            {
                MessageBox.Show("Будь ласка, виберіть елемент для редагування.");
                return;
            }

            var editWindow = new ChangeVegetableWindow(selectedVegetable);
            if (editWindow.ShowDialog() == true)
            {
                if (!vegetableList.Contains(selectedVegetable))
                {
                    ConfirmChangesToDB form = new ConfirmChangesToDB();

                    if (form.ShowDialog() == true)
                    {
                        VegetableDTO.UpdateVegetable(selectedVegetable);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            RefreshGardenData();
        }
        private void Del_Click(object sender, RoutedEventArgs e)
        {
            var selectedVegetable = GardenData.SelectedItem as Vegetable;

            if (selectedVegetable == null)
            {
                MessageBox.Show("Будь ласка, виберіть елемент для видалення.");
                return;
            }

            string name = selectedVegetable.VegetableName;
            string country = selectedVegetable.Country;
            int season = selectedVegetable.NumOfSeason;

            if (!vegetableList.Contains(selectedVegetable))
            {
                ConfirmChangesToDB form = new ConfirmChangesToDB();

                if (form.ShowDialog() == true)
                {
                    var vegetableToDelete = vegetableDB.Vegetables.FirstOrDefault(v => v.VegetableName == name && v.Country == country && v.NumOfSeason == season);

                    if (vegetableToDelete != null)
                    {
                        VegetableDTO.DeleteVegetable(vegetableToDelete);
                    }
                    else
                    {
                        MessageBox.Show("Не вдалося знайти елемент у базі даних.");
                    }

                }
                else
                {
                    return;
                }
            }

            else
            {
                var vegetableToDelete = vegetableList.FirstOrDefault(v => v.VegetableName == name && v.Country == country && v.NumOfSeason == season);
                vegetableList.Remove(vegetableToDelete);
            }

            RefreshGardenData();
        }
        private void RefreshGardenData()
        {
            var dbVegetables = vegetableDB.GetListFromDB();
            var combinedList = dbVegetables.Concat(vegetableList).ToList();

            GardenData.ItemsSource = combinedList;
        }
        private void SaveVegetablesToDatabase()
        {
            if (vegetableList.Any())
            {
                vegetableDB.SaveToDB(vegetableList);
            }
        }

        private void Add_Consignment_Click(object sender, RoutedEventArgs e)
        {
            AddConsigmentWindow addConsignmentWindow = new AddConsigmentWindow();
            if (addConsignmentWindow.ShowDialog() == true)
            {
                consignmentList.Add(addConsignmentWindow.NewConsignment);
                RefreshConsignmentData();
            }
        }

        private void Change_Consignment_Click(object sender, RoutedEventArgs e)
        {
            var selectedConsignment = ConsignmentData.SelectedItem as Consignment;

            if (selectedConsignment == null)
            {
                MessageBox.Show("Будь ласка, виберіть партію для редагування.");
                return;
            }

            var editWindow = new ChangeConsignmentWindow(selectedConsignment);
            if (editWindow.ShowDialog() == true)
            {
                if (!consignmentList.Contains(selectedConsignment))
                {
                    ConfirmChangesToDB form = new ConfirmChangesToDB();

                    if (form.ShowDialog() == true)
                    {
                        ConsignmentDTO.UpdateConsignment(selectedConsignment);
                    }
                    else
                    {
                        return;
                    }
                }
            }

            RefreshConsignmentData();
        }

        private void Del_Consignment_Click(object sender, RoutedEventArgs e)
        {
            var selectedConsignment = ConsignmentData.SelectedItem as Consignment;

            if (selectedConsignment == null)
            {
                MessageBox.Show("Будь ласка, виберіть партію для видалення.");
                return;
            }

            int vegetableId = selectedConsignment.VegetableId;
            decimal quantity = selectedConsignment.Quantity;
            string delivDate = selectedConsignment.DelivDate;

            if (!consignmentList.Contains(selectedConsignment))
            {
                ConfirmChangesToDB form = new ConfirmChangesToDB();

                if (form.ShowDialog() == true)
                {
                    var consignmentToDelete = consignmentDB.Consignments.FirstOrDefault(c =>
                        c.VegetableId == vegetableId &&
                        c.Quantity == quantity &&
                        c.DelivDate == delivDate);

                    if (consignmentToDelete != null)
                    {
                        ConsignmentDTO.DeleteConsignment(consignmentToDelete);
                    }
                    else
                    {
                        MessageBox.Show("Не вдалося знайти партію у базі даних.");
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                var consignmentToDelete = consignmentList.FirstOrDefault(c =>
                    c.VegetableId == vegetableId &&
                    c.Quantity == quantity &&
                    c.DelivDate == delivDate);

                consignmentList.Remove(consignmentToDelete);
            }

            RefreshConsignmentData();
        }
        private void RefreshConsignmentData()
        {
            var dbConsignments = consignmentDB.GetListFromDB();
            var combinedList = dbConsignments.Concat(consignmentList).ToList();
            ConsignmentData.ItemsSource = combinedList;
        }

        private void SaveConsignmentsToDatabase()
        {
            if (consignmentList.Any())
            {
                consignmentDB.SaveToDB(consignmentList);
            }
        }
    }
}
