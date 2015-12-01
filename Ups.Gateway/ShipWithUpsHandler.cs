using System;

namespace FedEx.Gateway
{
    using NServiceBus;
    using Shipping;

    public class ShipWithUpsHandler : IHandleMessages<ShipWithUps>
    {
        public IBus Bus { get; set; }

        public void Handle(ShipWithUps message)
        {
            Bus.Reply(new ShipWithUpsResponse
            {
               OrderId = message.OrderId
            });

            Console.WriteLine($"ups gateway{message.OrderId}");
        }
    }
}
