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
        StorageDTO storageDB;
        ConsignmentInStorageDTO consignmentsInStorageDB;
        public MainWindow()
        {
            InitializeComponent();

            MainGrid.Visibility = Visibility.Visible;
            GardenGrid.Visibility = Visibility.Collapsed;
            ConsignmentGrid.Visibility = Visibility.Collapsed;
            StorageGrid.Visibility = Visibility.Collapsed;

            vegetableDB = new VegetableDTO();
            consignmentDB = new ConsignmentDTO();
            storageDB = new StorageDTO();
            consignmentsInStorageDB = new ConsignmentInStorageDTO();
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
            ConsignmentsInStorageDataGrid.ItemsSource = null;
            ShortInfoToStorage.Text = "";
            RefreshStorageData();
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

            RefreshConsignmentDataInStorageTab();

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

        private void Accept_Consignment_Click(object sender, RoutedEventArgs e)
        {
            var selectedConsignment = ConsignmentsDataGrid.SelectedItem as Consignment;

            if (selectedConsignment == null)
            {
                MessageBox.Show("Будь ласка, виберіть партію.");
                return;
            }

            AcceptConisgnmentWindow acceptConisgnmentWindow = new AcceptConisgnmentWindow();
            if (acceptConisgnmentWindow.ShowDialog() == true)
            {
                Storage storage = acceptConisgnmentWindow.Storage;

                double totalWeight = consignmentsInStorageDB.GetTotalQuantityForStorage(storage.Id) + (double)selectedConsignment.Quantity;
                if (totalWeight > storage.Capacity)
                {
                    MessageBox.Show("Перевищено вмістимість складу.");
                    return;
                }
                ConfirmChangesToDB form = new ConfirmChangesToDB();

                if (form.ShowDialog() == true)
                {
                    storageDB.AcceptConsignment(storage.Id, selectedConsignment);

                    RefreshConsignmentDataInStorageTab();
                }
            }
        }

        private void Reject_Consignment_Click(object sender, RoutedEventArgs e)
        {
            var selectedConsignment = ConsignmentsInStorageDataGrid.SelectedItem as ConsignmentInStorage;

            if (selectedConsignment == null)
            {
                MessageBox.Show("Будь ласка, виберіть партію для списання!");
                return;
            }

            int id = selectedConsignment.Id;
            int storageId = selectedConsignment.StorageId;

            ConfirmChangesToDB form = new ConfirmChangesToDB();

            if (form.ShowDialog() == true)
            {
                consignmentsInStorageDB.RejectConsignment(id);

                RefreshConsignmentsInStorage(storageId);
                ShortInfoToStorage.Text = consignmentsInStorageDB.ToShortString(storageId);
            }
        }

        private void Add_Storage_Click(object sender, RoutedEventArgs e)
        {
            AddStorageWindow addStorageWindow = new AddStorageWindow();
            if (addStorageWindow.ShowDialog() == true)
            {
                Storage newStorage = addStorageWindow.NewStorage;

                try
                {
                    ConfirmChangesToDB form = new ConfirmChangesToDB();

                    if (form.ShowDialog() == true)
                    {
                        storageDB.AddStorage(newStorage);
                        RefreshStorageData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при додаванні складу.");
                }
            }
        }

        private void Del_Storage_Click(object sender, RoutedEventArgs e)
        {
            var selectedStorage = StorageData.SelectedItem as Storage;

            if (selectedStorage == null)
            {
                MessageBox.Show("Будь ласка, виберіть елемент для видалення.");
                return;
            }

            int id = selectedStorage.Id;

            ConfirmChangesToDB form = new ConfirmChangesToDB();

            if (form.ShowDialog() == true)
            {

                if (selectedStorage != null)
                {
                    storageDB.DeleteStorage(id);
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
            RefreshStorageData();
            ConsignmentsInStorageDataGrid.ItemsSource = null;
            ShortInfoToStorage.Text = "";
        }
        private void StorageData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedStorage = StorageData.SelectedItem as Storage;
            if (selectedStorage == null)
            {
                return;
            }

            RefreshConsignmentsInStorage(selectedStorage.Id);
            ShortInfoToStorage.Text = consignmentsInStorageDB.ToShortString(selectedStorage.Id);
        }
        private void RefreshStorageData()
        {
            var allStorages = storageDB.GetListOfStoragesFromDB();
            StorageData.ItemsSource = allStorages;
        }
        private void RefreshConsignmentsInStorage(int storageId)
        {
            var dbConsignmentsInStorage = consignmentsInStorageDB.GetListOfConsignmentsInStoragesFromDB(storageId);
            ConsignmentsInStorageDataGrid.ItemsSource = dbConsignmentsInStorage;
        }
        private void RefreshConsignmentDataInStorageTab()
        {
            var dbConsignments = consignmentDB.GetListFromDB();
            ConsignmentsDataGrid.ItemsSource = dbConsignments;
        }
    }
}
