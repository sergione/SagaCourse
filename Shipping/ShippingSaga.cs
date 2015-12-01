namespace Shipping
{
    using System;
    using Billing.Contracts;
    using NServiceBus.Saga;
    using Sales.Contracts;

    public class ShippingSaga : Saga<ShippingSagaData>,
        IAmStartedByMessages<OrderPlaced>,
        IAmStartedByMessages<OrderBilled>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingSagaData> mapper)
        {
            mapper.ConfigureMapping<OrderPlaced>(x => x.OrderId).ToSaga(x => x.OrderId);
            mapper.ConfigureMapping<OrderBilled>(x => x.OrderId).ToSaga(x => x.OrderId);
        }

        public void Handle(OrderPlaced message)
        {
            Data.OrderId = message.OrderId;
            Data.Placed = true;
            StartShipping();
        }

        public void Handle(OrderBilled message)
        {
            Data.OrderId = message.OrderId;
            Data.Billed = true;
            
            StartShipping();
        }

        void StartShipping()
        {
            if (!Data.Billed || !Data.Placed)
            {
                return;
            }

            Bus.SendLocal(new StartShippingCommand
            {
                OrderId = Data.OrderId
            });

            Console.WriteLine($"Start Shipping Subprocess: {Data.OrderId}");

            MarkAsComplete();
        }
    }

    public class ShippingSagaData : ContainSagaData
    {
        [Unique]
        public virtual string OrderId { get; set; }

        public virtual bool Placed { get; set; }

        public virtual bool Billed { get; set; }
    }
}
