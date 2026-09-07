namespace GestionarSarcini.Core;

/// <summary>
/// Abstractizeaza sursa de timp curent, pentru a face logica testabila
/// (in teste putem furniza un moment fix).
/// </summary>
public interface IProvidorTimp
{
    DateTime Acum { get; }
}

/// <summary>Implementarea reala, bazata pe ceasul sistemului.</summary>
public sealed class ProvidorTimpSistem : IProvidorTimp
{
    public DateTime Acum => DateTime.Now;
}
