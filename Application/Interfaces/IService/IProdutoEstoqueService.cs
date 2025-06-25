using Application.DTOs;
using Application.Inputs.ProdutoEstoqueInput;

namespace Application.Interfaces.IService
{
    public interface IProdutoEstoqueService
    {
        IEnumerable<ProdutoEstoqueDto> ObterTodosProdutoEstoqueDto();
        IEnumerable<ProdutoEstoqueDto> ObterTodosProdutoEstoquePorProdutoIdDto(int produtoId);
        IEnumerable<ProdutoEstoqueDto> ObterProdutoEstoqueDtoPorId(int id);
        void CadastrarProdutoEstoque(ProdutoEstoqueCadastroInput produtoEstoqueCadastroInput, string emailUsuarioLogado);
        void AlterarProdutoEstoque(ProdutoEstoqueAlteracaoInput produtoEstoqueAlteracaoInput, string emailUsuarioLogado);
        void DeletarProdutoEstoque(int id, string emailUsuarioLogado);
    }
}
