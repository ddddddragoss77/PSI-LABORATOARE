namespace GestionarSarcini.Core;

/// <summary>
/// Serviciul principal: gestioneaza colectia de sarcini
/// (adaugare, finalizare, stergere, filtrare, statistici).
/// </summary>
public class ManagerSarcini
{
    private readonly List<Sarcina> _sarcini = new();
    private readonly IProvidorTimp _timp;
    private int _urmatorulId = 1;

    public ManagerSarcini(IProvidorTimp? timp = null)
    {
        _timp = timp ?? new ProvidorTimpSistem();
    }

    /// <summary>Numarul total de sarcini din lista.</summary>
    public int Total => _sarcini.Count;

    /// <summary>Adauga o sarcina noua si returneaza obiectul creat.</summary>
    public Sarcina Adauga(string titlu, Prioritate prioritate = Prioritate.Medie)
    {
        var sarcina = new Sarcina(_urmatorulId, titlu, prioritate, _timp.Acum);
        _sarcini.Add(sarcina);
        _urmatorulId++;
        return sarcina;
    }

    /// <summary>Cauta o sarcina dupa id. Arunca exceptie daca nu exista.</summary>
    public Sarcina Obtine(int id)
    {
        return _sarcini.FirstOrDefault(s => s.Id == id)
            ?? throw new KeyNotFoundException($"Nu exista nicio sarcina cu id-ul {id}.");
    }

    /// <summary>Marcheaza sarcina drept finalizata.</summary>
    public void Finalizeaza(int id) => Obtine(id).Finalizeaza(_timp.Acum);

    /// <summary>Redeschide o sarcina finalizata.</summary>
    public void Redeschide(int id) => Obtine(id).Redeschide();

    /// <summary>Sterge o sarcina. Returneaza true daca a fost gasita si stearsa.</summary>
    public bool Sterge(int id) => _sarcini.RemoveAll(s => s.Id == id) > 0;

    /// <summary>Toate sarcinile, ordonate dupa prioritate (desc) apoi dupa data crearii.</summary>
    public IReadOnlyList<Sarcina> Toate() =>
        _sarcini
            .OrderByDescending(s => s.Prioritate)
            .ThenBy(s => s.CreataLa)
            .ThenBy(s => s.Id)
            .ToList();

    /// <summary>Doar sarcinile neterminate.</summary>
    public IReadOnlyList<Sarcina> InAsteptare() =>
        Toate().Where(s => !s.Finalizata).ToList();

    /// <summary>Doar sarcinile finalizate.</summary>
    public IReadOnlyList<Sarcina> Finalizate() =>
        Toate().Where(s => s.Finalizata).ToList();

    /// <summary>Sarcinile care contin textul dat in titlu (fara a tine cont de litere mari/mici).</summary>
    public IReadOnlyList<Sarcina> Cauta(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Toate();

        return Toate()
            .Where(s => s.Titlu.Contains(text.Trim(), StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    /// <summary>Procentul de sarcini finalizate (0-100). Returneaza 0 daca lista e goala.</summary>
    public double ProcentFinalizare()
    {
        if (_sarcini.Count == 0)
            return 0;

        return Math.Round(100.0 * _sarcini.Count(s => s.Finalizata) / _sarcini.Count, 1);
    }
}
