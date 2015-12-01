using System;

namespace FedEx.Gateway
{
    using System.Net.Http;
    using NServiceBus;
    using Shipping;

    public class ShipWithFedexHandler : IHandleMessages<ShipWithFedex>
    {
        public IBus Bus { get; set; }

        public void Handle(ShipWithFedex message)
        {
            var request = new HttpClient();
            var httpResponseMessage = request.GetAsync("http://localhost:8888/fedex/shipit").Result;
            Bus.Reply(new ShipWithFedexResponse
            {
               OrderId = message.OrderId
            });

            Console.WriteLine($"fedex gateway{message.OrderId}");
        }
    }
}
