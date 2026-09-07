using GestionarSarcini.Core;

namespace GestionarSarcini.Tests;

/// <summary>Provider de timp controlabil, folosit doar in teste.</summary>
internal sealed class ProvidorTimpFals : IProvidorTimp
{
    public ProvidorTimpFals(DateTime start) => Acum = start;

    public DateTime Acum { get; private set; }

    public void Avanseaza(TimeSpan interval) => Acum = Acum.Add(interval);
}
