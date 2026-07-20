using ProjetoIntegrador.Backend.Enums;

namespace ProjetoIntegrador.Backend.Modelos;

public class Alerta
{
    public int Id { get; private set; }
    public int IdUsuario { get; private set; }

    public DateTime DataHoraDisparo { get; private set; } = DateTime.UtcNow;
    public decimal Latitude { get; private set; }
    public decimal Longitude { get; private set; }
    public decimal PrecisaoGps { get; private set; }
    public Status Status { get; private set; } = Status.Ativo;

    protected Alerta()
    {
    }

    public Alerta(
        int idUsuario,
        DateTime dataHoraDisparo,
        decimal latitude,
        decimal longitude,
        decimal precisaoGps,
        Status? status)
    {
        InserirIdUsuario(idUsuario);
        InserirDataHoraDisparo(dataHoraDisparo);
        InserirLatitude(latitude);
        InserirLongitude(longitude);
        InserirPrecisaoGps(precisaoGps);
        InserirStatus(status);
    }

    private void InserirIdUsuario(int idUsuario)
    {
        if (idUsuario <= 0)
            throw new Exception("O ID do usuário deve ser maior que zero.");
        IdUsuario = idUsuario;
    }

    private void InserirDataHoraDisparo(DateTime dataHoraDisparo)
    {
        if (dataHoraDisparo > DateTime.UtcNow.AddMinutes(1))
            throw new Exception("A data e hora do disparo não pode estar no futuro.");

        DataHoraDisparo = dataHoraDisparo;
    }

    private void InserirLatitude(decimal latitude)
    {
        if (latitude is < -90 or > 90)
            throw new Exception("A latitude deve estar entre -90 e 90 graus.");

        Latitude = latitude;
    }

    private void InserirLongitude(decimal longitude)
    {
        if (longitude is < -180 or > 180)
            throw new Exception("A longitude deve estar entre -180 e 180 graus.");

        Longitude = longitude;
    }

    private void InserirPrecisaoGps(decimal precisaoGps)
    {
        if (precisaoGps < 0)
            throw new Exception("A precisão do GPS não pode ser um valor negativo.");

        PrecisaoGps = precisaoGps;
    }

    private void InserirStatus(Status? status)
    {
        switch (status)
        {
            case Status.FalsoAlarme:
                Status = Status.FalsoAlarme;
                break;
            case Status.Atendido:
                Status = Status.Atendido;
                break;
            default:
                Status = Status.Ativo;
                break;
        }
    }
}

public class AlertaDto
{
    public int IdUsuario { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal PrecisaoGps { get; set; }
    public Status? Status { get; set; }
}