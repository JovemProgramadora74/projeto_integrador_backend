using ProjetoIntegrador.Backend.Enums;

namespace ProjetoIntegrador.Backend.DTOs;

public class AlertaDto
{
    public int IdUsuario { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal PrecisaoGps { get; set; }
    public Status? Status { get; set; }
}