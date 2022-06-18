using System;
using Confluent.Kafka;

namespace SV.HRM.Logging.KafkaTarget
{
    public abstract class KafkaProducerAbstract : IDisposable
    {
        protected IProducer<string, byte[]> Producer;

        protected KafkaProducerAbstract(string brokers, string lingerMs)
        {
            var conf = new ProducerConfig
            {
                BootstrapServers = brokers,
                LingerMs = string.IsNullOrWhiteSpace(lingerMs) ? 0 : int.Parse(lingerMs),
                Acks = 0
                //CompressionType = CompressionType.Gzip
            };
            Producer = new ProducerBuilder<string, byte[]>(conf).Build();
        }

        public void Dispose()
        {
            Producer?.Flush();
            Producer?.Dispose();
        }

        public abstract void Produce(ref string topic, ref byte[] data);
    }
}