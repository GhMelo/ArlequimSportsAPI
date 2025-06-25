using Application.DTOs;
using Application.Inputs.UsuarioInput;

namespace Application.Interfaces.IService
{
    public interface IUsuarioService
    {
        UsuarioDto ObterUsuarioDtoPorEmail(string email);
        UsuarioDto ObterUsuarioDtoPorId(int id);
        IEnumerable<UsuarioDto> ObterTodosUsuariosDto();
        void CadastrarUsuario(UsuarioCadastroInput UsuarioCadastroInput);
        void AlterarUsuario(UsuarioAlteracaoInput UsuarioAlteracaoInput);
        void DeletarUsuario(int id);
    }
}
