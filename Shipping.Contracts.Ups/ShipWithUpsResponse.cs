namespace Shipping
{
    using NServiceBus;
    public class ShipWithUpsResponse : ICommand
    {
        public string OrderId { get; set; }
    }
}