
using System.ServiceModel;
using System.Xml.Serialization;
using Onnio.BcIntegrator.Models;
using SCOTransactionServices;

namespace Onnio.BcIntegrator
{
    public class Transaction
    {
        public Transaction() { }

        /*public void AddToTransaction(string receiptId,string barcode)
        {
            //Populate Item
            SCOAddToTransINItems item = new SCOAddToTransINItems();
            item.Barcode = barcode;
            item.EntryType = "0";


            SCOAddToTransaction request = new SCOAddToTransaction();
            request.receiptID = receiptId;
            request.sCOAddToTransINXML = new RootSCOAddToTransIN();
            request.sCOAddToTransINXML.SCOAddToTransINItems = new SCOAddToTransINItems[1];
            request.sCOAddToTransINXML.SCOAddToTransINItems[0] = item;
            //set the barcode and item type in xml string
            
            SCOAddToTrans_PortClient client = new SCOAddToTrans_PortClient();
            //client.ClientCredentials.UserName.UserName = "WebServices";
            //client.ClientCredentials.UserName.Password = "SuperSafePassword1!";
            
            var result = client.SCOAddToTransactionAsync(request).Result;
        }*/
        public void AddToTransaction(AddToTransInputDTO input)
        {
            var client = new SCOAddToTrans_PortClient();

            // Construct the request
            var request = new SCOAddToTransaction();
            request.authInfo = "{\"StoreID\":\"420\",\"StaffID\":\"\",\"TerminalID\":\"420SC01\",\"Token\":\"9b3fcaab-3b0f-4f7b-af2c-0f5ccb9639ae\"}";
            request.receiptID = input.receiptId;
             var item = new SCOAddToTransINItems
             {
                 EntryType = "Item",
                 Number = input.barcode,
                 Barcode = input.barcode,
                 Quantity = 1
             };
             request.sCOAddToTransINXML = new RootSCOAddToTransIN();
            request.sCOAddToTransINXML.SCOAddToTransINItems = new SCOAddToTransINItems[1];
            request.sCOAddToTransINXML.SCOAddToTransINItems[0] = item;



            // Call the service
            try
            {
                string xml = "";
                using (var stringwriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(this.GetType());
                    serializer.Serialize(stringwriter, this);
                    xml = stringwriter.ToString();
                }
                
                var response = client.SCOAddToTransactionAsync(request).Result;

                // Handle the response
                //Console.WriteLine("Response Code: " + response.responseCode);
                //Console.WriteLine("Error Text: " + response.errorText);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                // Close the client
                client.CloseAsync();
            }
        }
    }
}
