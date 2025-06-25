using Application.Inputs.EsporteModalidadeInput;

namespace Application.Interfaces.IService
{
    public interface IEsporteModalidadeService
    {
        IEnumerable<EsporteModalidadeDto> ObterTodosEsporteModalidadeDto();
        EsporteModalidadeDto ObterEsporteModalidadeDtoPorId(int id);
        void CadastrarEsporteModalidade(EsporteModalidadeCadastroInput esporteModalidadeCadastroInput);
        void AlterarEsporteModalidade(EsporteModalidadeAlteracaoInput esporteModalidadeAlteracaoInput);
        void DeletarEsporteModalidade(int id);
    }
}
