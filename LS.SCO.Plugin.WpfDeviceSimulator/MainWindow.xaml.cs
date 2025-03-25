using AutoMapper.Internal;
using LS.SCO.Plugin.Adapter.Controllers.Models;
using Newtonsoft.Json;
using System.CodeDom;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using static LS.SCO.Plugin.Adapter.Controllers.SampleController;

namespace LS.SCO.Plugin.WpfDeviceSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5027/api/sample";
        private string APIAddress => APIAddressTextBox.Text;

        public MainWindow()
        {
            InitializeComponent();

            _httpClient = new HttpClient();
            APIAddressTextBox.Text = BaseUrl;
        }

        private async void GetItemDetails_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateApiAddress()) return;
            ChangeButtonsVisibility(false);
            ResultTextBox.Text = string.Empty;

            try
            {
                string itemId = "10095";

                var response = await _httpClient.GetStringAsync($"{APIAddress}/item/{itemId}");

                // Format the JSON string
                var formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(response), Formatting.Indented);

                ResultTextBox.Text = $"Get Item Details Response:\n{formattedJson}";
            }
            catch (Exception ex)
            {
                ResultTextBox.Text = $"Error: {ex.Message}";
            }
            finally
            {
                ChangeButtonsVisibility(true);
            }
        }

        private async void GetCurrentTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateApiAddress()) return;
            ChangeButtonsVisibility(false);
            ResultTextBox.Text = string.Empty;

            try
            {
                var formattedJson = await GetCurrentTransactionJSON();

                ResultTextBox.Text = $"Get Current Transaction Response:\n{formattedJson}";
            }
            catch (Exception ex)
            {
                ResultTextBox.Text = $"Error: {ex.Message}";
            }
            finally
            {
                ChangeButtonsVisibility(true);
            }
        }

        private async void AddItemToTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateApiAddress()) return;
            ChangeButtonsVisibility(false);
            ResultTextBox.Text = string.Empty;

            try
            {
                var itemDetails = new GetItemDetailsInput
                {
                    BarCode = "10095"
                };

                var body = JsonConvert.SerializeObject(itemDetails);

                var content = new StringContent(body, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{APIAddress}/addItem", content);

                var responseBody = await response.Content.ReadAsStringAsync();

                // Format the JSON string
                var formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseBody), Formatting.Indented);

                ResultTextBox.Text = $"Add Item to Transaction Response:\n{formattedJson}";
            }
            catch (Exception ex)
            {
                ResultTextBox.Text = $"Error: {ex.Message}";
            }
            finally
            {
                ChangeButtonsVisibility(true);
            }
        }

        private async void CreateTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateApiAddress()) return;
            ChangeButtonsVisibility(false);
            ResultTextBox.Text = string.Empty;

            try
            {
                var response = await _httpClient.PostAsync($"{APIAddress}/createTransaction", null);

                var responseBody = await response.Content.ReadAsStringAsync();

                // Format the JSON string
                var formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseBody), Formatting.Indented);

                ResultTextBox.Text = $"Create Transaction Response:\n{formattedJson}";
            }
            catch (Exception ex)
            {
                ResultTextBox.Text = $"Error: {ex.Message}";
            }
            finally
            {
                ChangeButtonsVisibility(true);
            }
        }

        private async void PayCurrentTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateApiAddress()) return;
            ChangeButtonsVisibility(false);

            try
            {
                //var currTransJson = await GetCurrentTransactionJSON();

                var netAmountWithTax = 0m;

                //using (JsonDocument doc = JsonDocument.Parse(currTransJson))
                //{
                //    var root = doc.RootElement;
                //    netAmountWithTax = root
                //        .GetProperty("transaction")
                //        .GetProperty("netAmountWithTax")
                //        .GetDecimal();
                //}

                var dialog = new AmountDialog(netAmountWithTax);
                var result = dialog.ShowDialog();

                if (!result == true)
                    return;

                decimal amount = dialog.Amount;
                string tenderType = dialog.SelectedTenderType;

                MessageBox.Show($"Amount: {amount}\nTender Type: {tenderType}", "Input Result");

                ResultTextBox.Text = string.Empty;


                var myRequest = new PayForCurrentTransactionInput
                {
                    Value = amount,
                    TenderType = tenderType
                };

                var json = JsonConvert.SerializeObject(myRequest);

                // Create the HTTP content with the JSON string
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{APIAddress}/payForCurrentTransaction", content);

                var responseBody = await response.Content.ReadAsStringAsync();

                // Format the JSON string
                var formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseBody), Formatting.Indented);

                ResultTextBox.Text = $"Pay for Transaction Response:\n{formattedJson}";
            }
            catch (Exception ex)
            {
                ResultTextBox.Text = $"Error: {ex.Message}";
            }
            finally
            {
                ChangeButtonsVisibility(true);
            }
        }

        private async void FinishCurrentTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateApiAddress()) return;
            ChangeButtonsVisibility(false);
            ResultTextBox.Text = string.Empty;

            try
            {
                var response = await _httpClient.PostAsync($"{APIAddress}/finishCurrentTransaction", null);

                var responseBody = await response.Content.ReadAsStringAsync();

                // Format the JSON string
                var formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseBody), Formatting.Indented);

                ResultTextBox.Text = $"Finish Current Transaction Response:\n{formattedJson}";
            }
            catch (Exception ex)
            {
                ResultTextBox.Text = $"Error: {ex.Message}";
            }
            finally
            {
                ChangeButtonsVisibility(true);
            }
        }

        private async void VoidTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateApiAddress()) return;
            ChangeButtonsVisibility(false);
            ResultTextBox.Text = string.Empty;

            try
            {
                var response = await _httpClient.PostAsync($"{APIAddress}/voidTransaction", null);

                var responseBody = await response.Content.ReadAsStringAsync();

                // Format the JSON string
                var formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseBody), Formatting.Indented);

                ResultTextBox.Text = $"Void Transaction Response:\n{formattedJson}";
            }
            catch (Exception ex)
            {
                ResultTextBox.Text = $"Error: {ex.Message}";
            }
            finally
            {
                ChangeButtonsVisibility(true);
            }
        }

        private async void CancelActiveTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateApiAddress()) return;
            ChangeButtonsVisibility(false);
            ResultTextBox.Text = string.Empty;

            try
            {
                var response = await _httpClient.PostAsync($"{APIAddress}/cancelActiveTransaction", null);

                var responseBody = await response.Content.ReadAsStringAsync();

                // Format the JSON string
                var formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseBody), Formatting.Indented);

                ResultTextBox.Text = $"Void Transaction Response:\n{formattedJson}";
            }
            catch (Exception ex)
            {
                ResultTextBox.Text = $"Error: {ex.Message}";
            }
            finally
            {
                ChangeButtonsVisibility(true);
            }
        }

        private void ChangeButtonsVisibility(bool buttonsVisibility)
        {
            GetCurrTransactionBtn.IsEnabled = buttonsVisibility;
            GetItemDetailsBtn.IsEnabled = buttonsVisibility;
            CreateTransactionBtn.IsEnabled = buttonsVisibility;
            AddItemToTransBtn.IsEnabled = buttonsVisibility;
            PayCurrTransactionBtn.IsEnabled = buttonsVisibility;
            FinishCurrTransactionBtn.IsEnabled = buttonsVisibility;
            VoidTransactionBtn.IsEnabled = buttonsVisibility;
            CancelActiveTransactionBtn.IsEnabled = buttonsVisibility;
        }

        private async Task<string> GetCurrentTransactionJSON()
        {
            var response = await _httpClient.GetStringAsync($"{APIAddress}/getCurrentTransaction");

            // Format the JSON string
            var formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(response), Formatting.Indented);

            return formattedJson;
        }

        private bool ValidateApiAddress()
        {
            string apiAddress = APIAddressTextBox.Text;

            if (string.IsNullOrWhiteSpace(apiAddress))
            {
                MessageBox.Show("Please enter a valid API address (e.g., 127.0.0.1:8080 or localhost:5027/api/sample).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Add "http://" if no scheme is provided
            if (!apiAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !apiAddress.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                apiAddress = "http://" + apiAddress;
            }

            if (!Uri.TryCreate(apiAddress, UriKind.Absolute, out Uri uri))
            {
                MessageBox.Show("Please enter a valid API address (e.g., 127.0.0.1:8080 or localhost:5027/api/sample).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(uri.Host) || (!System.Net.IPAddress.TryParse(uri.Host, out _) && uri.Host != "localhost"))
            {
                MessageBox.Show("Please enter a valid IP address or hostname (e.g., 127.0.0.1 or localhost).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (uri.Port <= 0 || uri.Port > 65535)
            {
                MessageBox.Show("Please enter a valid port number (1-65535).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private async void PayCash_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateApiAddress()) return;
            ChangeButtonsVisibility(false);

            try
            {
                //var currTransJson = await GetCurrentTransactionJSON();

                var netAmountWithTax = 0m;

                //using (JsonDocument doc = JsonDocument.Parse(currTransJson))
                //{
                //    var root = doc.RootElement;
                //    netAmountWithTax = root
                //        .GetProperty("transaction")
                //        .GetProperty("netAmountWithTax")
                //        .GetDecimal();
                //}

                var dialog = new AmountDialog(netAmountWithTax);
                var result = dialog.ShowDialog();

                if (!result == true)
                    return;

                decimal amount = dialog.Amount;
                string tenderType = "1";

                //MessageBox.Show($"Amount: {amount}\nTender Type: {tenderType}", "Input Result");

                ResultTextBox.Text = string.Empty;


                var myRequest = new PayForCurrentTransactionInput
                {
                    Value = amount,
                    TenderType = tenderType
                };

                var json = JsonConvert.SerializeObject(myRequest);

                // Create the HTTP content with the JSON string
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{APIAddress}/payForCurrentTransactionExternal", content);

                var responseBody = await response.Content.ReadAsStringAsync();

                // Format the JSON string
                var formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseBody), Formatting.Indented);

                ResultTextBox.Text = $"Pay for Transaction Response:\n{formattedJson}";
            }
            catch (Exception ex)
            {
                ResultTextBox.Text = $"Error: {ex.Message}";
            }
            finally
            {
                ChangeButtonsVisibility(true);
            }
        }
    }
}