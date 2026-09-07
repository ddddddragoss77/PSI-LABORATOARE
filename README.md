# Gestionar de sarcini

Aplicație de consolă în C# / .NET 9 — manager de sarcini (to-do) cu prioritate,
finalizare/redeschidere, ștergere, filtrare, căutare și statistici de progres.

## Structură

```
src/
  GestionarSarcini.Core/    logica de business (fără I/O) — testabilă
  GestionarSarcini/         interfața de consolă
tests/
  GestionarSarcini.Tests/   teste unitare xUnit (28 de teste)
```

## Rulare

```bash
dotnet run --project src/GestionarSarcini
```

Comenzi: `add <titlu> [| prioritate]`, `done <id>`, `undo <id>`, `del <id>`,
`list`, `pending`, `find <text>`, `stats`, `help`, `exit`.

## Testare

```bash
dotnet test
```
