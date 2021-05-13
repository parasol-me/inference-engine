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

        //figure 7.10
        private bool TruthTableQueryRecursive(HornFormKnowledgeBase kb, string query, List<string> symbols, HornFormKnowledgeBase model)
        {
            if (symbols.Count.Equals(0))
            {
                HornClause singleSymbolQuery = new HornClause(null, null, new HashSet<string>() {query});
                HornFormKnowledgeBase singleSymbolKb =
                    new HornFormKnowledgeBase(new List<HornClause>() {singleSymbolQuery});
                return (isPropositionTrue(kb, model)) ? isPropositionTrue(singleSymbolKb, model) : true;
            }
        
            //recursively assign true/false values to all symbols, filling model
            HornClause trueAssignedProposition = new HornClause(null, null, new HashSet<string>(){symbols.First()});
            HornClause falseAssignedProposition = new HornClause(null, null, new HashSet<string>(){symbols.First()});
            symbols.RemoveAt(0);
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

        private HornFormKnowledgeBase evaluateKB(HornFormKnowledgeBase inputKb)
        {
            foreach (var clause in inputKb.Clauses)
            {
                if (clause.IsSymbolFact(sentence.ConjunctSymbols))
            }
        }
    }
}