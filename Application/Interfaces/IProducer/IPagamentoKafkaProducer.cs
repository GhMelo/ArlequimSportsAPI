using Application.DTOs;

namespace Application.Interfaces.IProducer
{
    public interface IPagamentoKafkaProducer
    {
        Task EnviarMensagemAsync(MensagemPagamentoDto mensagem);
    }
}
