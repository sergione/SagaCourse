namespace Shipping
{
    using NServiceBus;
    public class StartShippingCommand : ICommand
    {
        public string OrderId { get; set; }
    }
}
