﻿using System;
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
    /// Interaction logic for AcceptConisgnmentWindow.xaml
    /// </summary>
    public partial class AcceptConisgnmentWindow : Window
    {
        public Storage Storage { get; set; }
        StorageDTO storageDB = new StorageDTO();
        public AcceptConisgnmentWindow()
        {
            InitializeComponent();

            StorageComboBox.ItemsSource = storageDB.GetListOfStoragesFromDB();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (StorageComboBox.SelectedItem is Storage selectedStorage)
            {
                Storage = selectedStorage;
            }
            else
            {
                MessageBox.Show("Будь ласка, оберіть склад!");
            }
            this.DialogResult = true;
            this.Close();
        }
    }
}
