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
            var symbols = kb.Clauses.SelectMany(clause => 
                    new HashSet<string>(clause.Value.ConjunctSymbols){clause.Value.ImplicationSymbol, query})
                .Where(symbol => symbol != null)
                .ToHashSet();

            return TruthTableQueryRecursive(kb, query, symbols, new TruthTableModel(new Dictionary<string, bool?>()));
        }

        //figure 7.10
        private bool TruthTableQueryRecursive(HornFormKnowledgeBase kb, string query, HashSet<string> symbols, TruthTableModel model)
        {
            if (symbols.Count == 0)
            {
                var queryInModel = model.SentenceToTruthValue[query];
                var queryValue = queryInModel ?? false;
                
                return (isPropositionTrue(kb, model)) ? queryValue : true;
            }

            var symbol = symbols.First();
            symbols.Remove(symbol);
            
            //recursively assign true/false values to all symbols, filling model
            var trueAssignedModel = 
                new TruthTableModel(new Dictionary<string, bool?>(model.SentenceToTruthValue)
                {
                    {symbol, true}
                });
            
            var falseAssignedModel = 
                new TruthTableModel(new Dictionary<string, bool?>(model.SentenceToTruthValue)
                {
                    {symbol, false}
                });
            
            
            return TruthTableQueryRecursive(kb, query, symbols, trueAssignedModel) 
                                             && TruthTableQueryRecursive(kb, query, symbols, falseAssignedModel);
        }

        private bool isPropositionTrue(HornFormKnowledgeBase kbOrSentence, TruthTableModel model)
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