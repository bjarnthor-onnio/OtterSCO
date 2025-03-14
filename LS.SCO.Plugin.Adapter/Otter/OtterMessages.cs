
namespace LS.SCO.Plugin.Adapter.Otter
{
    public static class OtterMessages
    {

        public static Models.FromPOS.Init InitMessage(string lane = "auto",string store = "auto", string posVersion = "auto")
        {
            return
                new Models.FromPOS.Init
                    {
                        jsonrpc = "2.0",
                        method = "Init",
                        @params = new Models.FromPOS.InitParams()
                        {
                            laneNumber = lane,
                            storeNumber = store,
                            posVersion = posVersion,
                        },
                        id = Guid.NewGuid().ToString()
                    };
        }

        public static Models.FromPOS.product ProductError(string id, string message)
        {
            return
                new Otter.Models.FromPOS.product
                {
                    result = new Otter.Models.FromPOS.ProductResult()
                    {
                        message = message,
                        successful = false,
                        productExceptions = new Otter.Models.FromPOS.ProductExceptions()
                        {
                            message = message
                        }
                    },
                    id = id
                };
        }

    }
}
