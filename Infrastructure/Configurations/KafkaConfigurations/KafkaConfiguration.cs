namespace Infrastructure.Configurations.KafkaConfigurations
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; } = null!;
        public string EmailTopic { get; set; } = null!;
    }
}
