namespace Shipping
{
    using System;
    using NServiceBus;
    public class StartShippingHandler : IHandleMessages<StartShippingCommand>
    {
        public void Handle(StartShippingCommand message)
        {
            Console.WriteLine($"Handle Start Shipping Command:{message.OrderId}");
        }
    }
}
