using Application.Inputs.ProdutoEstoqueInput;

namespace Application.Interfaces.IService
{
    internal interface IProdutoEstoqueService
    {
        IEnumerable<ProdutoEstoqueDto> ObterTodosProdutoEstoqueDto();
        IEnumerable<ProdutoEstoqueDto> ObterTodosProdutoEstoquePorProdutoIdDto(int produtoId);
        ProdutoEstoqueDto ObterProdutoEstoqueDtoPorId(int id);
        void CadastrarProdutoEstoque(ProdutoEstoqueCadastroInput produtoEstoqueCadastroInput, string nomeUsuarioLogado);
        void AlterarProdutoEstoque(ProdutoEstoqueAlteracaoInput produtoEstoqueAlteracaoInput);
        void DeletarProdutoEstoque(int id);
    }
}
