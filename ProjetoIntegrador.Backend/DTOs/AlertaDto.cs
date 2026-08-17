using ProjetoIntegrador.Backend.Enums;

namespace ProjetoIntegrador.Backend.DTOs;

public class AlertaDto
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal PrecisaoGps { get; set; }
}