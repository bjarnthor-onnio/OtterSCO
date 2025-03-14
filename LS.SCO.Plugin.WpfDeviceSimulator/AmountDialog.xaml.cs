using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
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

namespace LS.SCO.Plugin.WpfDeviceSimulator
{
    /// <summary>
    /// Interaction logic for AmountDialog.xaml
    /// </summary>
    public partial class AmountDialog : Window
    {
        public decimal Amount { get; private set; }
        public string SelectedTenderType { get; private set; }

        public AmountDialog(decimal ammount)
        {
            InitializeComponent();
            LoadTenderTypes();

            AmountTextBox.Text = ammount.ToString();
        }

        private void LoadTenderTypes()
        {
            // Populate the combo box with tender types
            var tenderTypes = new List<string> { "Card", "CreditCard" };
            TenderTypeComboBox.ItemsSource = tenderTypes;

            // Set the default selection
            TenderTypeComboBox.SelectedIndex = 0;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Try to parse the entered amount
            if (decimal.TryParse(AmountTextBox.Text, out decimal amount))
            {
                Amount = amount;
                SelectedTenderType = TenderTypeComboBox.SelectedItem?.ToString();
                DialogResult = true; // Close the dialog and return success
                Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid decimal value for the amount.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Close the dialog without saving
            Close();
        }
    }
}
