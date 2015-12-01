namespace Billing
{
    using System;
    using Contracts;
    using NServiceBus;
    using Sales.Contracts;
    public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
    {
        public IBus Bus { get; set; }

        public void Handle(OrderPlaced message)
        {
            Bus.Publish(new OrderBilled
            {
                OrderId = message.OrderId
            });

            Console.WriteLine($"Handle Order Placed in Billing Service: {message.OrderId}");
        }
    }
}
