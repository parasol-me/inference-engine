using System;
using System.Collections.Generic;
using System.Linq;

namespace assignment2
{
    public class TruthTable
    {
        
        public bool DoesEntail(HornFormKnowledgeBase kb, string query)
        {
            List<string> symbols = new List<string>();
            symbols.Add(query);
            foreach (HornClause hc in kb.Clauses)
            {
                if (hc.FinalImplication != null && (bool) !hc.FinalImplication)
                {
                    if (!symbols.Contains(hc.ImplicationSymbol))
                        symbols.Add(hc.ImplicationSymbol);
                }
                {
                    foreach (var conjunctSymbol in hc.ConjunctSymbols)
                    {
                        if (!symbols.Contains(conjunctSymbol))
                            symbols.Add(conjunctSymbol);
                    }
                }
                
            }
            return TruthTableQueryRecursive(kb, query, symbols, new HornFormKnowledgeBase(new List<HornClause>()));
        }

        private bool TruthTableQueryRecursive(HornFormKnowledgeBase kb, string query, List<string> symbols, HornFormKnowledgeBase model)
        {
            if (symbols.Count.Equals(0))
            {
                return (isPropositionTrue(kb, model)) ? isPropositionTrue(query, model) : true;
            }
        
            //recursively assign true/false values to all symbols, filling model
            HornClause proposition = new HornClause(null, null, new HashSet<string>(){symbols.First()});
            symbols.RemoveAt(0);
            proposition.SetTrue(true);
            model.Clauses.Add(proposition);
            return (TruthTableQueryRecursive(kb, query, symbols, model) 
                                            // && TruthTableQueryRecursive(kb, query, symbols, model.Clauses.Add(proposition.SetTrue(true)))
                                            );
        }

        private bool isPropositionTrue(HornFormKnowledgeBase kb, HornFormKnowledgeBase model)
        {
            throw new NotImplementedException();
        }
        
        //convert KB to true/false values
    }
}