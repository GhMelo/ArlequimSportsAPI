using System.ComponentModel.DataAnnotations;

namespace Application.Inputs.EsporteModalidadeInput
{
    public class EsporteModalidadeAlteracaoInput
    {
        [Required(ErrorMessage = "IdEsporteModalidade é obrigatório.")]
        public int IdEsporteModalidade { get; set; }
        [Required(ErrorMessage = "Descrição é obrigatório.")]
        public string Descricao { get; set; } = null!;
    }
}
