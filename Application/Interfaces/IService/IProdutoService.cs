using Application.DTOs;
using Application.Inputs.ProdutoInput;

namespace Application.Interfaces.IService
{
    public interface IProdutoService
    {
        IEnumerable<ProdutoDto> ObterTodosProdutoDto();
        IEnumerable<ProdutoDto> ObterProdutoPorEsporteModalidade(int esporteModalidadeId);
        IEnumerable<ProdutoDto> ObterProdutoPorNome(string nome);
        ProdutoDto ObterProdutoPorId(int id);
        void CadastrarProduto(ProdutoCadastroInput produtoCadastroInput);
        void AlterarProduto(ProdutoAlteracaoInput produtoAlteracaoInput);
        void DeletarProduto(int id);
    }
}
