using GestionarSarcini.Core;

namespace GestionarSarcini.Tests;

public class ManagerSarciniTests
{
    private readonly ProvidorTimpFals _timp = new(new DateTime(2026, 9, 7, 9, 0, 0));
    private readonly ManagerSarcini _manager;

    public ManagerSarciniTests()
    {
        _manager = new ManagerSarcini(_timp);
    }

    [Fact]
    public void Adauga_IncrementeazaIdSiTotal()
    {
        Sarcina prima = _manager.Adauga("A");
        Sarcina aDoua = _manager.Adauga("B");

        Assert.Equal(1, prima.Id);
        Assert.Equal(2, aDoua.Id);
        Assert.Equal(2, _manager.Total);
    }

    [Fact]
    public void Adauga_FoloseresteMomentulDinProvidorulDeTimp()
    {
        Sarcina s = _manager.Adauga("A");
        Assert.Equal(_timp.Acum, s.CreataLa);
    }

    [Fact]
    public void Obtine_IdInexistent_AruncaKeyNotFoundException()
    {
        Assert.Throws<KeyNotFoundException>(() => _manager.Obtine(99));
    }

    [Fact]
    public void Finalizeaza_MarcheazaSarcinaCorecta()
    {
        _manager.Adauga("A");
        _manager.Adauga("B");

        _manager.Finalizeaza(2);

        Assert.False(_manager.Obtine(1).Finalizata);
        Assert.True(_manager.Obtine(2).Finalizata);
    }

    [Fact]
    public void Finalizeaza_FoloseresteMomentulCurentAlProvidorului()
    {
        _manager.Adauga("A");
        _timp.Avanseaza(TimeSpan.FromHours(3));

        _manager.Finalizeaza(1);

        Assert.Equal(_timp.Acum, _manager.Obtine(1).FinalizataLa);
    }

    [Fact]
    public void Redeschide_ReadduceSarcinaInAsteptare()
    {
        _manager.Adauga("A");
        _manager.Finalizeaza(1);

        _manager.Redeschide(1);

        Assert.Single(_manager.InAsteptare());
        Assert.Empty(_manager.Finalizate());
    }

    [Fact]
    public void Sterge_SarcinaExistenta_ReturneazaTrueSiReduceTotalul()
    {
        _manager.Adauga("A");

        bool rezultat = _manager.Sterge(1);

        Assert.True(rezultat);
        Assert.Equal(0, _manager.Total);
    }

    [Fact]
    public void Sterge_SarcinaInexistenta_ReturneazaFalse()
    {
        Assert.False(_manager.Sterge(42));
    }

    [Fact]
    public void Toate_OrdoneazaDupaPrioritateDescApoiDupaData()
    {
        _manager.Adauga("scazuta", Prioritate.Scazuta);
        _timp.Avanseaza(TimeSpan.FromMinutes(1));
        _manager.Adauga("ridicata-veche", Prioritate.Ridicata);
        _timp.Avanseaza(TimeSpan.FromMinutes(1));
        _manager.Adauga("ridicata-noua", Prioritate.Ridicata);
        _timp.Avanseaza(TimeSpan.FromMinutes(1));
        _manager.Adauga("medie", Prioritate.Medie);

        string[] titluri = _manager.Toate().Select(s => s.Titlu).ToArray();

        Assert.Equal(new[] { "ridicata-veche", "ridicata-noua", "medie", "scazuta" }, titluri);
    }

    [Fact]
    public void InAsteptareSiFinalizate_ImpartCorectSarcinile()
    {
        _manager.Adauga("A");
        _manager.Adauga("B");
        _manager.Adauga("C");
        _manager.Finalizeaza(2);

        Assert.Equal(2, _manager.InAsteptare().Count);
        Assert.Single(_manager.Finalizate());
    }

    [Theory]
    [InlineData("git", 1)]
    [InlineData("GIT", 1)]
    [InlineData("scrie", 2)]
    [InlineData("inexistent", 0)]
    public void Cauta_GasesteDupaSubsirFaraCazLitere(string text, int asteptat)
    {
        _manager.Adauga("Invata git");
        _manager.Adauga("Scrie cod");
        _manager.Adauga("Scrie teste");

        Assert.Equal(asteptat, _manager.Cauta(text).Count);
    }

    [Fact]
    public void Cauta_TextGol_ReturneazaToateSarcinile()
    {
        _manager.Adauga("A");
        _manager.Adauga("B");

        Assert.Equal(2, _manager.Cauta("   ").Count);
    }

    [Fact]
    public void ProcentFinalizare_ListaGoala_ReturneazaZero()
    {
        Assert.Equal(0, _manager.ProcentFinalizare());
    }

    [Fact]
    public void ProcentFinalizare_CalculeazaCorectSiRotunjeste()
    {
        _manager.Adauga("A");
        _manager.Adauga("B");
        _manager.Adauga("C");
        _manager.Finalizeaza(1);

        // 1 din 3 = 33.333... -> 33.3
        Assert.Equal(33.3, _manager.ProcentFinalizare());
    }
}
