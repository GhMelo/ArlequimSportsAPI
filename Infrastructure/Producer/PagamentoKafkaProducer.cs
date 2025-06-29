using System.Text.Json;
using Application.DTOs;
using Application.Interfaces.IProducer;
using Confluent.Kafka;
using Infrastructure.Configurations.KafkaConfigurations;
using Microsoft.Extensions.Options;

namespace Infrastructure.Producer
{
    public class PagamentoKafkaProducer : IPagamentoKafkaProducer
    {
        private readonly KafkaSettings _settings;

        public PagamentoKafkaProducer(IOptions<KafkaSettings> options)
        {
            _settings = options.Value;
        }

        public async Task EnviarMensagemAsync(MensagemPagamentoDto mensagem)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _settings.BootstrapServers
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            var value = JsonSerializer.Serialize(mensagem);

            var result = await producer.ProduceAsync("fila-pagamento", new Message<Null, string> { Value = value });

            Console.WriteLine($"Mensagem de pagamento enviada: {result.TopicPartitionOffset}");
        }
    }

}
