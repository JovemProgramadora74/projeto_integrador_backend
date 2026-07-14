using ProjetoIntegrador.Backend.Dados;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Controladores;

public class AlertaControlador(AppDbContexto _contexto)
{
    public async void AddAsync(Alerta alerta)
    {
       await _contexto.Alertas.AddAsync(alerta);
       await _contexto.SaveChangesAsync();
       return alerta.Id;
    }
}

