using Application.Inputs.TipoOperacaoInput;

namespace Application.Interfaces.IService
{
    public interface ITipoOperacaoService
    {
        IEnumerable<TipoOperacaoDto> ObterTodosTipoOperacaoDto();
        TipoOperacaoDto ObterTipoOperacaoPorId(int id);
        void CadastrarTipoOperacao(TipoOperacaoInputCadastro tipoOperacaoCadastroInput);
        void AlterarTipoOperacao(TipoOperacaoInputAlteracao tipoOperacaoAlteracaoInput);
        void DeletarTipoOperacao(int id);
    }
}
