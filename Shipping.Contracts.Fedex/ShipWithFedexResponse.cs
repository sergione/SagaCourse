namespace Shipping
{
    using NServiceBus;

    public class ShipWithFedexResponse : ICommand
    {
        public string OrderId { get; set; }
    }
}