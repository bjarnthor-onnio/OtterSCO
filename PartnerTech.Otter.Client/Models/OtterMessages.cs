namespace PartnerTech.Otter.Client.Models
{
    public static class OtterMessages
    {

        public static ResponseModels.Init InitMessage(string posId="",string lane = "auto",string store = "auto", string posVersion = "auto")
        {
            
            return
                new ResponseModels.Init
                    {
                        jsonrpc = "2.0",
                        method = "Init",
                        @params = new ResponseModels.InitParams()
                        {
                            laneNumber = lane,
                            storeNumber = store,
                            posVersion = posVersion,
                            posId = posId
                        },
                        id = Guid.NewGuid().ToString()
                    };
        }

        public static ResponseModels.product ProductError(string id, string message)
        {
            return
                new ResponseModels.product
                {
                    result = new ResponseModels.ProductResult()
                    {
                        message = message,
                        successful = false,
                        productExceptions = new ResponseModels.ProductExceptions()
                        {
                            message = message
                        }
                    },
                    id = id
                };
        }

    }
}
