using System.Text.RegularExpressions;

namespace ProjetoIntegrador.Backend.Modelos;

public class Login
{
    protected Login()
    {
    }

    public Login(string email, string senha)
    {
        InserirEmail(email);
        InserirSenha(senha);
    }

    public string Email { get; private set; } = string.Empty;
    public string Senha { get; private set; } = string.Empty;

    private void InserirEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new Exception("E-mail deve ser informado!");
        if (!Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.IgnoreCase))
            throw new Exception("O e-mail inserido é inválido!");
        Email = email;
    }

    private void InserirSenha(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            throw new Exception("Senha deve ser informada!");
        if (senha.Length < 8)
            throw new Exception("A senha deve conter no mínimo 8 caracteres!");
        Senha = senha;
    }
}

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}