namespace Sales
{
    using System;
    using CustomerCare.Contracts;
    using NServiceBus;
    public class CustomerMadePreferredHandler : IHandleMessages<CustomerMadePreferred>
    {
        public void Handle(CustomerMadePreferred message)
        {
            Console.WriteLine($"handle customer made preferred event: {message.CustomerId}");
        }
    }
}
