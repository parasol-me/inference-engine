#nullable enable
using System;
using System.Collections.Generic;

namespace assignment2
{
    public class HornClause
    {
        public HornClause(string? implicationSymbol, bool? finalImplication, HashSet<string> conjunctedSymbols)
        {
            ImplicationSymbol = implicationSymbol;
            FinalImplication = finalImplication;
            ConjunctSymbols = conjunctedSymbols;
        }

        public bool? FinalImplication { get; }
        
        public string? ImplicationSymbol { get; }

        public HashSet<string> ConjunctSymbols;
    }
}