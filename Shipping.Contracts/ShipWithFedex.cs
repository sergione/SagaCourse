namespace Shipping
{
    using NServiceBus;
    public class ShipWithFedex : ICommand
    {
        public string OrderId { get; set; }
    }
}
