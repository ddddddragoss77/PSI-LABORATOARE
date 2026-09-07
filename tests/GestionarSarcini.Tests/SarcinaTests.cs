using GestionarSarcini.Core;

namespace GestionarSarcini.Tests;

public class SarcinaTests
{
    private static readonly DateTime Moment = new(2026, 9, 7, 10, 0, 0);

    [Fact]
    public void Constructor_NormalizeazaTitlulSiSeteazaValoriInitiale()
    {
        var sarcina = new Sarcina(1, "  Scrie raportul  ", Prioritate.Ridicata, Moment);

        Assert.Equal(1, sarcina.Id);
        Assert.Equal("Scrie raportul", sarcina.Titlu);
        Assert.Equal(Prioritate.Ridicata, sarcina.Prioritate);
        Assert.False(sarcina.Finalizata);
        Assert.Null(sarcina.FinalizataLa);
        Assert.Equal(Moment, sarcina.CreataLa);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_TitluGol_AruncaArgumentException(string? titlu)
    {
        Assert.Throws<ArgumentException>(() => new Sarcina(1, titlu!, Prioritate.Medie, Moment));
    }

    [Fact]
    public void Constructor_IdNepozitiv_AruncaExceptie()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Sarcina(0, "test", Prioritate.Medie, Moment));
    }

    [Fact]
    public void Finalizeaza_SeteazaStareaSiData()
    {
        var sarcina = new Sarcina(1, "test", Prioritate.Medie, Moment);
        DateTime cand = Moment.AddHours(2);

        sarcina.Finalizeaza(cand);

        Assert.True(sarcina.Finalizata);
        Assert.Equal(cand, sarcina.FinalizataLa);
    }

    [Fact]
    public void Finalizeaza_DeDouaOri_AruncaInvalidOperationException()
    {
        var sarcina = new Sarcina(1, "test", Prioritate.Medie, Moment);
        sarcina.Finalizeaza(Moment);

        Assert.Throws<InvalidOperationException>(() => sarcina.Finalizeaza(Moment));
    }

    [Fact]
    public void Redeschide_DupaFinalizare_RevineLaStareaInitiala()
    {
        var sarcina = new Sarcina(1, "test", Prioritate.Medie, Moment);
        sarcina.Finalizeaza(Moment);

        sarcina.Redeschide();

        Assert.False(sarcina.Finalizata);
        Assert.Null(sarcina.FinalizataLa);
    }

    [Fact]
    public void Redeschide_SarcinaNefinalizata_AruncaInvalidOperationException()
    {
        var sarcina = new Sarcina(1, "test", Prioritate.Medie, Moment);

        Assert.Throws<InvalidOperationException>(() => sarcina.Redeschide());
    }

    [Fact]
    public void SchimbaTitlu_TitluInvalid_AruncaSiPastreazaValoareaVeche()
    {
        var sarcina = new Sarcina(1, "titlu initial", Prioritate.Medie, Moment);

        Assert.Throws<ArgumentException>(() => sarcina.SchimbaTitlu("  "));
        Assert.Equal("titlu initial", sarcina.Titlu);
    }

    [Fact]
    public void ToString_ReflectaStarea()
    {
        var sarcina = new Sarcina(5, "demo", Prioritate.Scazuta, Moment);
        Assert.Equal("[ ] #5 (Scazuta) demo", sarcina.ToString());

        sarcina.Finalizeaza(Moment);
        Assert.Equal("[x] #5 (Scazuta) demo", sarcina.ToString());
    }
}
