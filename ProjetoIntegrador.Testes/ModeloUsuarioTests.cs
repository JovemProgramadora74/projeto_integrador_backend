using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Testes;

public class ModeloUsuarioTests
{
    private const string Nome = "Gabriel Alves";
    private const string Email = "gabriel@email.com";
    
    [Fact]
    public void Deve_Criar_Usuario_Com_Dados_Validos()
    {
        var usuario = new Usuario(Nome, Email);

        Assert.Equal(Nome, usuario.Nome);
        Assert.Equal(Email, usuario.Email);
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Nome_Esta_Vazio()
    {
        Assert.Throws<ArgumentException>(() =>
            new Usuario(string.Empty, Email)
        );
    }
    
    [Fact]
    public void Deve_Lancar_Excecao_Quando_Nome_Esta_Invalido()
    {
        Assert.Throws<ArgumentException>(() =>
            new Usuario("G4bri1el", Email)
        );
    }
    
    [Fact]
    public void Deve_Lancar_Excecao_Quando_Email_Esta_Vazio()
    {
        Assert.Throws<ArgumentException>(() =>
            new Usuario(Nome, string.Empty)
        );
    }
    
    [Fact]
    public void Deve_Lancar_Excecao_Quando_Email_Esta_Invalido()
    {
        Assert.Throws<ArgumentException>(() =>
            new Usuario(Nome, "gabriel@email")
        );
    }
}