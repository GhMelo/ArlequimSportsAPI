using Application.Inputs.ProdutoInput;

namespace Application.Interfaces.IService
{
    public interface IProdutoService
    {
        IEnumerable<ProdutoDto> ObterTodosProdutoDto();
        ProdutoDto ObterProdutoPorEsporteModalidade(string esporteModalidade);
        ProdutoDto ObterProdutoPorNome(string nome);
        ProdutoDto ObterProdutoPorId(int id);
        void CadastrarProduto(ProdutoCadastroInput produtoCadastroInput, string nomeUsuarioLogado);
        void AlterarProduto(ProdutoAlteracaoInput produtoAlteracaoInput);
        void DeletarProduto(int id);
    }
}
