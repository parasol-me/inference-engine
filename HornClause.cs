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
            bool isTrue;
        }

        public bool? FinalImplication { get; }
        
        public string? ImplicationSymbol { get; }

        public HashSet<string> ConjunctSymbols { get; }

        public void SetTrue(bool isTrue) { this.isTrue = isTrue; }

        public bool IsSymbolFact(string symbol)
        {
            return ConjunctSymbols.Contains(symbol) && FinalImplication == true;
        }
    }
}