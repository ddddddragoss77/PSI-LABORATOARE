using GestionarSarcini.Core;

namespace GestionarSarcini.ConsoleApp;

/// <summary>
/// Interfata de linie de comanda pentru managerul de sarcini.
/// Toata logica de business se afla in proiectul GestionarSarcini.Core.
/// </summary>
internal static class Program
{
    private static readonly ManagerSarcini Manager = new();

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        SeamanaDateDemo();

        Console.WriteLine("=== Gestionar de sarcini ===");
        Console.WriteLine("Comenzi: add | done <id> | undo <id> | del <id> | list | pending | find <text> | stats | help | exit");
        Console.WriteLine();

        bool ruleaza = true;
        while (ruleaza)
        {
            Console.Write("> ");
            string intrare = Console.ReadLine()?.Trim() ?? "exit";
            if (intrare.Length == 0)
                continue;

            string[] parti = intrare.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string comanda = parti[0].ToLowerInvariant();
            string argument = parti.Length > 1 ? parti[1] : string.Empty;

            try
            {
                ruleaza = ExecutaComanda(comanda, argument);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare: {ex.Message}");
            }
        }
    }

    private static bool ExecutaComanda(string comanda, string argument)
    {
        switch (comanda)
        {
            case "add":
                Adauga(argument);
                break;
            case "done":
                Manager.Finalizeaza(ParseId(argument));
                Console.WriteLine("Sarcina a fost marcata drept finalizata.");
                break;
            case "undo":
                Manager.Redeschide(ParseId(argument));
                Console.WriteLine("Sarcina a fost redeschisa.");
                break;
            case "del":
                Console.WriteLine(Manager.Sterge(ParseId(argument))
                    ? "Sarcina a fost stearsa."
                    : "Nu exista nicio sarcina cu acest id.");
                break;
            case "list":
                Afiseaza(Manager.Toate());
                break;
            case "pending":
                Afiseaza(Manager.InAsteptare());
                break;
            case "find":
                Afiseaza(Manager.Cauta(argument));
                break;
            case "stats":
                Console.WriteLine($"Total: {Manager.Total} | " +
                                  $"In asteptare: {Manager.InAsteptare().Count} | " +
                                  $"Finalizate: {Manager.Finalizate().Count} | " +
                                  $"Progres: {Manager.ProcentFinalizare()}%");
                break;
            case "help":
                Console.WriteLine("Comenzi: add | done <id> | undo <id> | del <id> | list | pending | find <text> | stats | help | exit");
                break;
            case "exit":
            case "quit":
                return false;
            default:
                Console.WriteLine($"Comanda necunoscuta: '{comanda}'. Scrie 'help' pentru lista comenzilor.");
                break;
        }

        return true;
    }

    private static void Adauga(string argument)
    {
        // Format acceptat: "titlu" sau "titlu | ridicata"
        string titlu = argument;
        Prioritate prioritate = Prioritate.Medie;

        int separator = argument.LastIndexOf('|');
        if (separator >= 0)
        {
            titlu = argument[..separator].Trim();
            string textPrioritate = argument[(separator + 1)..].Trim();
            if (Enum.TryParse(textPrioritate, ignoreCase: true, out Prioritate p))
                prioritate = p;
        }

        Sarcina s = Manager.Adauga(titlu, prioritate);
        Console.WriteLine($"Adaugata: {s}");
    }

    private static int ParseId(string argument)
    {
        if (int.TryParse(argument.Trim(), out int id))
            return id;

        throw new FormatException($"'{argument}' nu este un id valid.");
    }

    private static void Afiseaza(IReadOnlyList<Sarcina> sarcini)
    {
        if (sarcini.Count == 0)
        {
            Console.WriteLine("(nicio sarcina)");
            return;
        }

        foreach (Sarcina s in sarcini)
            Console.WriteLine("  " + s);
    }

    private static void SeamanaDateDemo()
    {
        Manager.Adauga("Configureaza repository-ul GitHub", Prioritate.Ridicata);
        Manager.Adauga("Instaleaza Git Bash si VS Code", Prioritate.Medie);
        Manager.Adauga("Scrie testele unitare", Prioritate.Ridicata);
        Manager.Adauga("Fa commit si push pe branch-ul de feature", Prioritate.Scazuta);
        Manager.Finalizeaza(1);
    }
}
