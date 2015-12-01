namespace Shipping
{
    using NServiceBus;
    public class ShipWithUps : ICommand
    {
        public string OrderId { get; set; }
    }
}
