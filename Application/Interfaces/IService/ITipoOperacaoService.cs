using Application.DTOs;
using Application.Inputs.TipoOperacaoInput;

namespace Application.Interfaces.IService
{
    public interface ITipoOperacaoService
    {
        public IEnumerable<TipoOperacaoDto> ObterTodosTipoOperacaoDto();
        public TipoOperacaoDto ObterTipoOperacaoPorId(int id);
        public void CadastrarTipoOperacao(TipoOperacaoInputCadastro tipoOperacaoCadastroInput);
        public void AlterarTipoOperacao(TipoOperacaoInputAlteracao tipoOperacaoAlteracaoInput);
        public void DeletarTipoOperacao(int id);
    }
}
