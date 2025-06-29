using System.Text.Json;
using Application.DTOs;
using Application.Interfaces.IProducer;
using Confluent.Kafka;
using Infrastructure.Configurations.KafkaConfigurations;
using Microsoft.Extensions.Options;

namespace Infrastructure.Producer
{
    public class EmailKafkaProducer : IEmailKafkaProducer
    {
        private readonly KafkaSettings _settings;

        public EmailKafkaProducer(IOptions<KafkaSettings> options)
        {
            _settings = options.Value;
        }

        public async Task EnviarMensagemAsync(MensagemEmailDto mensagem)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _settings.BootstrapServers
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            var value = JsonSerializer.Serialize(mensagem);

            var result = await producer.ProduceAsync(_settings.EmailTopic, new Message<Null, string> { Value = value });

            Console.WriteLine($"Mensagem enviada para Kafka: {result.TopicPartitionOffset}");
        }
    }
}
