using Confluent.Kafka;
using SV.HRM.Logging.ExceptionCustom;
using SV.HRM.Logging.StaticConfig;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.ComponentModel;
using System.Text;

namespace SV.HRM.Logging.KafkaTarget
{
    [Target("KafkaAppender")]
    public class KafkaAppender : TargetWithLayout
    {
        private KafkaProducerAbstract _producer;
        private static readonly object Locker = new object();
        private bool _recovering;

        public KafkaAppender()
        {
            if (string.IsNullOrEmpty(Brokers))
            {
                Brokers = StaticConfiguration.KafkaServer;
                Console.WriteLine($"Brokers:{Brokers} ");
            }
        }

        /// <summary>
        ///     Gets or sets the layout used to format topic of log messages.
        /// </summary>
        [RequiredParameter]
        [DefaultValue("${callsite:className=true:fileName=false:includeSourcePath=false:methodName=true}")]
        public Layout Topic { get; set; }

        /// <summary>
        ///     Gets or sets the layout used to format log messages.
        /// </summary>
        [DefaultValue("${longdate}|${level:uppercase=true}|${logger}|${message}")]
        public override Layout Layout { get; set; }

        /// <summary>
        ///     Kafka brokers with comma-separated
        /// </summary>
        //[RequiredParameter]
        public string Brokers { get; set; }

        /// <summary>
        ///     Gets or sets debugging mode enabled
        /// </summary>
        public bool Debug { get; set; } = false;

        /// <summary>
        ///     Gets or sets async or sync mode
        /// </summary>
        public bool Async { get; set; } = false;

        public string LingerMs { get; set; }

        /// <summary>
        ///     initializeTarget
        /// </summary>
        protected override void InitializeTarget()
        {
            base.InitializeTarget();
            try
            {
                if (string.IsNullOrEmpty(Brokers))
                {
                    //throw new BrokerNotFoundException("Broker is not found");
                    Console.WriteLine("Broker is not found");
                    throw new BrokerNotFoundException("Broker is not found");
                }

                if (_producer != null) return;
                lock (Locker)
                {
                    if (_producer != null) return;
                    if (Async)
                        _producer = new KafkaProducerAsync(Brokers, LingerMs);
                    else
                        _producer = new KafkaProducerSync(Brokers, LingerMs);
                }
            }
            catch (Exception ex)
            {
                if (Debug) Console.WriteLine(ex.ToString());

                base.CloseTarget();
            }
        }

        /// <summary>
        ///     disposing the target
        /// </summary>
        protected override void CloseTarget()
        {
            base.CloseTarget();
            try
            {
                _producer?.Dispose();
            }
            catch (Exception ex)
            {
                if (Debug) Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        ///     log event will be appended over broker
        /// </summary>
        /// <param name="logEvent"></param>
        protected override void Write(LogEventInfo logEvent)
        {
            try
            {
                var topic = Topic.Render(logEvent);
                var logMessage = Layout.Render(logEvent);
                var bytes = Encoding.ASCII.GetBytes(logMessage);
                _producer.Produce(ref topic, ref bytes);
            }
            catch (ProduceException<Null, string> ex)
            {
                if (ex.Error.IsFatal)
                {
                    if (!_recovering)
                    {
                        lock (Locker)
                        {
                            if (!_recovering)
                            {
                                _recovering = true;
                                try
                                {
                                    _producer?.Dispose();
                                }
                                catch (Exception ex2)
                                {
                                    if (Debug) Console.WriteLine(ex2.ToString());
                                }

                                if (Async)
                                    _producer = new KafkaProducerAsync(Brokers, LingerMs);
                                else
                                    _producer = new KafkaProducerSync(Brokers, LingerMs);

                                _recovering = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Debug) Console.WriteLine(ex.ToString());
            }
        }
    }
}