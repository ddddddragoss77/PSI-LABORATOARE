namespace GestionarSarcini.Core;

/// <summary>Reprezinta o sarcina (task) din lista de lucru.</summary>
public class Sarcina
{
    public int Id { get; init; }

    public string Titlu { get; private set; }

    public Prioritate Prioritate { get; private set; }

    public bool Finalizata { get; private set; }

    public DateTime CreataLa { get; init; }

    public DateTime? FinalizataLa { get; private set; }

    public Sarcina(int id, string titlu, Prioritate prioritate, DateTime creataLa)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Id-ul trebuie sa fie pozitiv.");

        Id = id;
        Titlu = ValideazaTitlu(titlu);
        Prioritate = prioritate;
        CreataLa = creataLa;
        Finalizata = false;
    }

    /// <summary>Marcheaza sarcina drept finalizata la momentul indicat.</summary>
    public void Finalizeaza(DateTime cand)
    {
        if (Finalizata)
            throw new InvalidOperationException("Sarcina este deja finalizata.");

        Finalizata = true;
        FinalizataLa = cand;
    }

    /// <summary>Redeschide o sarcina finalizata.</summary>
    public void Redeschide()
    {
        if (!Finalizata)
            throw new InvalidOperationException("Sarcina nu este finalizata.");

        Finalizata = false;
        FinalizataLa = null;
    }

    /// <summary>Schimba titlul sarcinii.</summary>
    public void SchimbaTitlu(string titluNou) => Titlu = ValideazaTitlu(titluNou);

    /// <summary>Schimba prioritatea sarcinii.</summary>
    public void SchimbaPrioritate(Prioritate prioritateNoua) => Prioritate = prioritateNoua;

    private static string ValideazaTitlu(string titlu)
    {
        if (string.IsNullOrWhiteSpace(titlu))
            throw new ArgumentException("Titlul sarcinii nu poate fi gol.", nameof(titlu));

        return titlu.Trim();
    }

    public override string ToString()
    {
        string stare = Finalizata ? "[x]" : "[ ]";
        return $"{stare} #{Id} ({Prioritate}) {Titlu}";
    }
}
