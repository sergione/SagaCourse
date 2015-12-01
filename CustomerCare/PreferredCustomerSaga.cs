namespace CustomerCare
{
    using System;
    using Contracts;
    using NServiceBus.Saga;
    using Sales.Contracts;

    public class PreferredCustomerSaga : 
        Saga<PreferredCustomerSagaData>,
        IAmStartedByMessages<OrderPlaced>,
        IHandleTimeouts<PrefferedCustomerOrderTimeout>

    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<PreferredCustomerSagaData> mapper)
        {
            mapper.ConfigureMapping<OrderPlaced>(x => x.CustomerId).ToSaga(x => x.CustomerId);
        }

        public void Handle(OrderPlaced message)
        {
            Data.CustomerId = message.CustomerId;
            Data.RunningTotal += message.Amount;

            RequestTimeout(new TimeSpan(0, 0, 20), new PrefferedCustomerOrderTimeout
            {
                Amount = message.Amount
            });

            if (Data.RunningTotal > 5000)
            {
                Bus.Publish(new CustomerMadePreferred
                {
                    CustomerId = Data.CustomerId
                });

                Console.WriteLine("became preffered");
            }
        }

        public void Timeout(PrefferedCustomerOrderTimeout state)
        {
            Data.RunningTotal -= state.Amount;

            if (Data.RunningTotal < 5000)
            {
                Console.WriteLine("customer demoted");
                Bus.Publish(new CustomerDemoted
                {
                    CustomerId = Data.CustomerId
                });
            }
        }
    }

    public class PrefferedCustomerOrderTimeout
    {
        public double Amount { get; set; }
    }

    public class PreferredCustomerSagaData : ContainSagaData
    {
        public virtual string CustomerId { get; set; }

        public virtual double RunningTotal { get; set; }
    }
}
