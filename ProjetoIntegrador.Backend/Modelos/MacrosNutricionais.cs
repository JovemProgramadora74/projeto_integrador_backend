namespace ProjetoIntegrador.Backend.Modelos;

public class MacrosNutricionais
{
    protected MacrosNutricionais()
    {
    }

    public MacrosNutricionais(int proteina, int carboidrato, int gordura)
    {
        if (proteina < 0 || carboidrato < 0 || gordura < 0)
            throw new Exception("Os valores de macros não podem ser negativos!");

        ProteinaPorcentagem = proteina;
        CarboidratosPorcentagem = carboidrato;
        GordurasPorcentagem = gordura;
    }

    public int ProteinaPorcentagem { get; private set; }
    public int CarboidratosPorcentagem { get; private set; }
    public int GordurasPorcentagem { get; private set; }
}

public class MacrosDto
{
    public int ProteinaPorcentagem { get; set; }
    public int CarboidratosPorcentagem { get; set; }
    public int GordurasPorcentagem { get; set; }
}