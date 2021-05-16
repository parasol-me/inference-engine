#nullable enable
using System.Collections.Generic;

namespace assignment2
{
    public class HornClause
    {
        public HornClause(string? implicationSymbol, bool? finalImplication, HashSet<string> conjunctSymbols)
        {
            ImplicationSymbol = implicationSymbol;
            FinalImplication = finalImplication;
            ConjunctSymbols = conjunctSymbols;
        }

        public static HornClause AsFact(string factSymbol)
        {
            return new(null, true, new HashSet<string>() {factSymbol});
        }

        public bool? FinalImplication { get; }

        public string? ImplicationSymbol { get; }

        public HashSet<string> ConjunctSymbols { get; }

        public bool IsSymbolFact(string symbol)
        {
            return ConjunctSymbols.Contains(symbol) && FinalImplication == true;
        }
    }
}