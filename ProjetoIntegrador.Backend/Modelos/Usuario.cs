using System.Text.RegularExpressions;

namespace ProjetoIntegrador.Backend.Modelos;

public class Usuario
{
    protected Usuario()
    {
    }

    public Usuario(string nome, string email, string senha, string username)
    {
        InserirNome(nome);
        InserirEmail(email);
        InserirSenha(senha);
        InserirUsername(username);
    }

    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Senha { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public DateTime CriadoEm { get; private set; } =  DateTime.UtcNow;


    private void InserirNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("Nome não pode estar vazio!");

        if (nome.Length < 3)
            throw new Exception("Nome precisa ter no mínimo 3 caracteres!");

        if (Regex.IsMatch(nome, @"[^\p{L}\s]", RegexOptions.IgnoreCase))
            throw new Exception("Nome contém caracteres invalidos!");

        Nome = nome;
    }

    private void InserirEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new Exception("O email não pode estar vazio!");

        if (!Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.IgnoreCase))
            throw new Exception("O email não é valido!");

        Email = email;
    }

    private void InserirSenha(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            throw new Exception("A senha não pode estar vazia!");

        if (senha.Length < 8)
            throw new Exception("A senha precisa ter no mínimo 8 caracteres!");

        var cost = 16;
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(senha, workFactor: cost);
        
        Senha = hashedPassword;
    }

    private void InserirUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new Exception("O username não pode estar vazio!");

        if (username.Length < 3 || username.Length > 20)
            throw new Exception("O username precisa ter entre 3 a 20 caracteres!");

        Username = username;
    }
}

public class UsuarioCadastroDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}

public class UsuarioLoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}