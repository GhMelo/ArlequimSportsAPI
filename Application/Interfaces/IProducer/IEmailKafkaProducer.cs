using Application.DTOs;

namespace Application.Interfaces.IProducer
{
    public interface IEmailKafkaProducer
    {
        Task EnviarMensagemAsync(MensagemEmailDto mensagem);
    }
}
