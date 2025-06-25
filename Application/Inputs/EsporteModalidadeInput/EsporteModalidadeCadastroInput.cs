using System.ComponentModel.DataAnnotations;

namespace Application.Inputs.EsporteModalidadeInput
{
    public class EsporteModalidadeCadastroInput
    {

        [Required(ErrorMessage = "Descrição é obrigatório.")]
        public string Descricao { get; set; } = null!;
    }
}
