namespace Shipping
{
    using System;
    using NServiceBus;
    using NServiceBus.Saga;

    public class ArrangeShippingSaga : Saga<ArrangeShippingSagaData>,
        IAmStartedByMessages<StartShippingCommand>,
        IHandleMessages<ShipWithFedexResponse>,
        IHandleMessages<ShipWithUpsResponse>,
        IHandleTimeouts<FedexTimeout>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ArrangeShippingSagaData> mapper)
        {
            mapper.ConfigureMapping<StartShippingCommand>(x => x.OrderId).ToSaga(x => x.OrderId);
        }

        public void Handle(StartShippingCommand message)
        {
            Data.OrderId = message.OrderId;

            Bus.Send(new ShipWithFedex
            {
                OrderId = message.OrderId
            });

            RequestTimeout<FedexTimeout>(new TimeSpan(0, 0, FedEx.TimeoutInSeconds));
            Console.WriteLine($"Arrange Shipping Details: {message.OrderId}");
        }

        public void Handle(ShipWithFedexResponse message)
        {
            Console.WriteLine($"fedex response:{Data.OrderId}");
            MarkAsComplete();
        }

        public void Handle(ShipWithUpsResponse message)
        {
            Console.WriteLine($"ups response: {Data.OrderId}");
            MarkAsComplete();
        }

        public void Timeout(FedexTimeout state)
        {
            Bus.Send(new ShipWithUps
            {
                OrderId = Data.OrderId
            });

            Console.WriteLine($"fedex timeout: {Data.OrderId}");
        }
    }

    public class FedexTimeout
    {
    }

    public class ArrangeShippingSagaData : ContainSagaData
    {
        public virtual string OrderId { get; set; }
    }
}
