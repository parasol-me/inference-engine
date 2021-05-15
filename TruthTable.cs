using System;
using System.Collections.Generic;
using System.Linq;

namespace assignment2
{
    public class TruthTable
    {
        
        //figure 7.10
        public bool DoesEntail(HornFormKnowledgeBase kb, string query)
        {
            var symbols = kb.Clauses.SelectMany(clause => new HashSet<string>(clause.ConjunctSymbols){clause.ImplicationSymbol, query})
                .Where(symbol => symbol != null)
                .ToHashSet();

            var model = 
            
            return TruthTableQueryRecursive(kb, query, symbols, );
        }

        //figure 7.10
        private bool TruthTableQueryRecursive(HornFormKnowledgeBase kb, string query, HashSet<string> symbols, HornFormKnowledgeBase model)
        {
            if (symbols.Count == 0)
            {
                var singleSymbolQuery = new HornClause(null, null, new HashSet<string>() {query});
                var singleSymbolKb =
                    new HornFormKnowledgeBase(new List<HornClause>() {singleSymbolQuery});
                return (isPropositionTrue(kb, model)) ? isPropositionTrue(singleSymbolKb, model) : true;
            }

            var symbol = symbols.First();
            
            //recursively assign true/false values to all symbols, filling model
            var trueAssignedProposition = new HornClause(null, null, new HashSet<string>(){symbol});
            var falseAssignedProposition = new HornClause(null, null, new HashSet<string>(){symbol});
            symbols.Remove(symbol);
            trueAssignedProposition.SetTrue(true);
            falseAssignedProposition.SetTrue(false);
            
            model.Clauses.Add(trueAssignedProposition);
            return (TruthTableQueryRecursive(kb, query, symbols, model) 
                                            // && TruthTableQueryRecursive(kb, query, symbols, model.Clauses.Add(falseAssignedProposition))
                                            );
        }

        private bool isPropositionTrue(HornFormKnowledgeBase kbOrSentence, HornFormKnowledgeBase model)
        {
            
            //alpha implies beta = true?
            //if sentence holds within model, true
            //foreach sentence in kbOrSentence
                //foreach model in model
                    //if sentence holds, return true
                    return true;

        }

        // private HornFormKnowledgeBase evaluateKB(HornFormKnowledgeBase inputKb)
        // {
        //     foreach (var clause in inputKb.Clauses)
        //     {
        //         if (clause.IsSymbolFact(sentence.ConjunctSymbols))
        //     }
        // }
    }
}