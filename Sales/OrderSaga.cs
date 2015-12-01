using System;

namespace Sales
{
    using Contracts;
    using Messages;
    using NServiceBus;
    using NServiceBus.Saga;
    using NServiceBus.Timeout.Core;

    public class OrderSaga : Saga<OrderSagaData>,
        IAmStartedByMessages<StartOrder>, 
        IHandleMessages<PlaceOrder>,
        IHandleMessages<CancelOrder>,
        IHandleTimeouts<TimeoutData>
    {
        public void Handle(StartOrder message)
        {
            Data.OrderId = message.OrderId;
            Console.WriteLine("Handle Start Order:" + Data.OrderId);
            RequestTimeout<TimeoutData>(new TimeSpan(0, 0, 30));
        }

        public void Timeout(TimeoutData timeoutData)
        {
            Console.WriteLine("Order abandoned");
            var orderAbandoned = new OrderAbandoned()
            {
                OrderId = Data.OrderId
            };

            Bus.Publish(orderAbandoned);
        }

        public void Handle(PlaceOrder message)
        {
            Console.WriteLine("Handle Place Order: " +  Data.OrderId);
            var orderPlaced = new OrderPlaced
            {
                OrderId = Data.OrderId
            };

            Bus.Publish(orderPlaced);
        }

        public void Handle(CancelOrder message)
        {
            Console.WriteLine("Handle Cancel Order: " + Data.OrderId);
            var orderCanceled = new OrderCanceled
            {
                OrderId = Data.OrderId
            };

            Bus.Publish(orderCanceled);
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
        {
            mapper.ConfigureMapping<StartOrder>(x => x.OrderId).ToSaga(x => x.OrderId);
            mapper.ConfigureMapping<PlaceOrder>(x => x.OrderId).ToSaga(x => x.OrderId);
            mapper.ConfigureMapping<CancelOrder>(x => x.OrderId).ToSaga(x => x.OrderId);
        }
    }

    public class OrderSagaData : ContainSagaData
    {
        public virtual string OrderId { get; set; }
    }
}
