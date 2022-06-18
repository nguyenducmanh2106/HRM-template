using System;
using Confluent.Kafka;

namespace SV.HRM.Logging.KafkaTarget
{
    public class KafkaProducerAsync : KafkaProducerAbstract
    {
        public KafkaProducerAsync(string brokers, string lingerMs) : base(brokers, lingerMs)
        {
        }

        public override void Produce(ref string topic, ref byte[] data)
        {
            Producer.ProduceAsync(topic, new Message<string, byte[]>
            {
                Key = Guid.NewGuid().ToString(),
                Value = data
            });
        }
    }
}