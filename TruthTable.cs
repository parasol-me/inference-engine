using System;
using System.Collections.Generic;
using System.Linq;

namespace assignment2
{
    public class TruthTable
    {
        private int entailedCount = 0;
        //figure 7.10
        public QueryResult DoesEntail(HornFormKnowledgeBase kb, string query)
        {
            var symbols = kb.Clauses.SelectMany(clause => 
                    new HashSet<string>(clause.Value.ConjunctSymbols){clause.Value.ImplicationSymbol, query})
                .Where(symbol => symbol != null)
                .ToHashSet();
            
            return new QueryResult(
                TruthTableQueryRecursive(kb, query, symbols, new TruthTableModel(new Dictionary<string, bool>())),
                new HashSet<string>() {entailedCount.ToString()}, null, null);
        }

        //figure 7.10
        private bool TruthTableQueryRecursive(HornFormKnowledgeBase kb, string query, HashSet<string> symbols, TruthTableModel model)
        {
            if (symbols.Count == 0)
            {
               var result = isPropositionTrue(kb, model) ? model.SentenceToTruthValue[query] : true;
               if (result)
                   entailedCount++;
               return result;

            }

            var symbol = symbols.First();
            symbols.Remove(symbol);
            
            //recursively assign true/false values to all symbols, filling model
            var trueAssignedModel = 
                new TruthTableModel(new Dictionary<string, bool>(model.SentenceToTruthValue)
                {
                    {symbol, true}
                });
            
            var falseAssignedModel = 
                new TruthTableModel(new Dictionary<string, bool>(model.SentenceToTruthValue)
                {
                    {symbol, false}
                });
            
            
            return TruthTableQueryRecursive(kb, query, symbols, trueAssignedModel) 
                                             && TruthTableQueryRecursive(kb, query, symbols, falseAssignedModel);
        }

        private bool isPropositionTrue(HornFormKnowledgeBase kbOrSentence, TruthTableModel model)
        {
            //if sentence holds within model, true
            foreach (var keyValuePair in kbOrSentence.Clauses)
            {
                var sentence = keyValuePair.Key;
                var clause = keyValuePair.Value; //todo evaluate complex clauses (non-atomic sentences)
                var exists = model.SentenceToTruthValue.ContainsKey(sentence);
                
                if (exists && model.SentenceToTruthValue[sentence] != true)
                    return false;
            }

            return true;
        }

        // private HornFormKnowledgeBase evaluateKB(HornFormKnowledgeBase inputKb) //populate TT with complex sentences
        // {
        //     foreach (var clause in inputKb.Clauses)
        //     {
        //         if (clause.IsSymbolFact(sentence.ConjunctSymbols))
        //     }
        // }
    }
}