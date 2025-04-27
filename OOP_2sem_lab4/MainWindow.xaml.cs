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
        public MainWindow()
        {
            InitializeComponent();

            MainGrid.Visibility = Visibility.Visible;
            GardenGrid.Visibility = Visibility.Collapsed;
            ConsignmentGrid.Visibility = Visibility.Collapsed;
            StorageGrid.Visibility = Visibility.Collapsed;

            vegetableDB = new VegetableDTO();
            List<Vegetable> garden = vegetableDB.Vegetables.ToList();
            string strGarden = "";
            foreach (var vegetable in garden)
                strGarden += "Name: " + vegetable.VegetableName + " " + "Country: " + vegetable.Country + " " + "Season: " + vegetable.NumOfSeason + "\n";

            gardenText.Text = strGarden;
        }

        private void Button_Window_Main_Click(object sender, RoutedEventArgs e)
        {
            if (vegetableList != null)
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

            MainGrid.Visibility = Visibility.Visible;
            GardenGrid.Visibility = Visibility.Collapsed;
            ConsignmentGrid.Visibility = Visibility.Collapsed;
            StorageGrid.Visibility = Visibility.Collapsed;
        }

        private void Button_Window_Garden_Click(object sender, RoutedEventArgs e)
        {
            RefreshGardenData();
            MainGrid.Visibility = Visibility.Collapsed;
            GardenGrid.Visibility = Visibility.Visible;
            ConsignmentGrid.Visibility = Visibility.Collapsed;
            StorageGrid.Visibility = Visibility.Collapsed;
        }

        private void Button_Window_Сonsignment_Click(object sender, RoutedEventArgs e)
        {
            if (vegetableList != null)
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
            if (vegetableList != null)
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

            var editableVegetable = new Vegetable
            {
                Id = selectedVegetable.Id,
                VegetableName = selectedVegetable.VegetableName,
                Country = selectedVegetable.Country,
                NumOfSeason = selectedVegetable.NumOfSeason
            };

            var editWindow = new ChangeVegetableWindow(editableVegetable);
            if (editWindow.ShowDialog() == true)
            {
                if (!vegetableList.Contains(selectedVegetable))
                {
                    ConfirmChangesToDB form = new ConfirmChangesToDB();

                    if (form.ShowDialog() == true)
                    {
                        selectedVegetable.VegetableName = editableVegetable.VegetableName;
                        selectedVegetable.Country = editableVegetable.Country;
                        selectedVegetable.NumOfSeason = editableVegetable.NumOfSeason;

                        VegetableDTO.UpdateVegetable(selectedVegetable);
                    }
                    else
                    {
                        return;
                    }
                }

                else
                {
                    selectedVegetable.VegetableName = editableVegetable.VegetableName;
                    selectedVegetable.Country = editableVegetable.Country;
                    selectedVegetable.NumOfSeason = editableVegetable.NumOfSeason;
                }
                GardenData.Items.Refresh();
            }
        }
        private void Del_Click(object sender, RoutedEventArgs e)
        {
            var selectedVegetable = GardenData.SelectedItem as Vegetable;

            if (selectedVegetable == null)
            {
                MessageBox.Show("Будь ласка, виберіть елемент для видалення.");
                return;
            }

            dynamic selectedVegetableDynamic = selectedVegetable;
            string name = selectedVegetableDynamic.VegetableName;
            string country = selectedVegetableDynamic.Country;
            int season = selectedVegetableDynamic.NumOfSeason;

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
    }
}
