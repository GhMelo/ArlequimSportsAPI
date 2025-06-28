using Application.DTOs;
using Application.Inputs.EsporteModalidadeInput;
using Application.Interfaces.IService;
using Domain.Entity;
using Domain.Interfaces.IRepository;

namespace Application.Services
{
    public class EsporteModalidadeService : IEsporteModalidadeService
    {
        private readonly IEsporteModalidadeRepository _esporteModalideRepository;
        public EsporteModalidadeService(IEsporteModalidadeRepository esporteModalideRepository)
        {
            _esporteModalideRepository = esporteModalideRepository;
        }
        public void AlterarEsporteModalidade(EsporteModalidadeAlteracaoInput esporteModalidadeAlteracaoInput)
        {
            var esporteModalidade = _esporteModalideRepository.ObterPorId(esporteModalidadeAlteracaoInput.IdEsporteModalidade);
            esporteModalidade.Descricao = esporteModalidadeAlteracaoInput.Descricao;
            _esporteModalideRepository.Alterar(esporteModalidade);
        }

        public void CadastrarEsporteModalidade(EsporteModalidadeCadastroInput esporteModalidadeCadastroInput)
        {
            var esporteModalidade = new EsporteModalidade()
            {
                Descricao = esporteModalidadeCadastroInput.Descricao
            };
            _esporteModalideRepository.Cadastrar(esporteModalidade);
        }

        public void DeletarEsporteModalidade(int id)
        {
            _esporteModalideRepository.Deletar(id);
        }

        public EsporteModalidadeDto ObterEsporteModalidadeDtoPorId(int id)
        {
            var esporteModalidade = _esporteModalideRepository.ObterPorId(id);
            return new EsporteModalidadeDto()
            {
                Id = esporteModalidade.Id,
                Descricao = esporteModalidade.Descricao
            };
        }

        public IEnumerable<EsporteModalidadeDto> ObterTodosEsporteModalidadeDto()
        {
            var esporteModalidade = _esporteModalideRepository.ObterTodos();
            return esporteModalidade.Select(x => new EsporteModalidadeDto()
            {
                Id = x.Id,
                Descricao = x.Descricao
            });
        }
    }
}
