using System;
using System.Collections.Generic;
using System.Linq;

namespace assignment2
{
    public class TruthTable
    {
        private int _entailedCount;

        //figure 7.10
        public QueryResult DoesEntail(HornFormKnowledgeBase kb, string query)
        {
            _entailedCount = 0;
            var symbols = kb.Clauses.SelectMany(clause => 
                    new HashSet<string>(clause.Value.ConjunctSymbols){clause.Value.ImplicationSymbol, query})
                .Where(symbol => symbol != null)
                .ToHashSet();

            return new QueryResult(
                TruthTableQueryRecursive(kb, query, symbols, new TruthTableModel(new Dictionary<string, bool>())),
                new HashSet<string>() {_entailedCount.ToString()}, null, null);
        }

        //figure 7.10
        private bool TruthTableQueryRecursive(HornFormKnowledgeBase kb, string query, HashSet<string> symbols, TruthTableModel model)
        {
            if (symbols.Count == 0)
            {
                var doesKbHoldInModel = kb.Clauses.All(clauseKvp =>
                {
                    var (sentence, clause) = clauseKvp;
                    
                    var holds = false;

                    if (clause.FinalImplication.HasValue)
                    {
                        var existsInModel = model.SentenceToTruthValue.TryGetValue(sentence, out var isTrueInModel);
                        holds = !existsInModel || isTrueInModel == clause.FinalImplication;
                    }
                    else if (clause.ImplicationSymbol != null)
                    {
                        holds = true;
                        var conjunctAreTrueInModel = clause.ConjunctSymbols.All(s =>
                            model.SentenceToTruthValue.ContainsKey(s) && model.SentenceToTruthValue[s]);
                        var implicationSymbolIsFalseInModel =
                            model.SentenceToTruthValue.ContainsKey(clause.ImplicationSymbol)
                            && !model.SentenceToTruthValue[clause.ImplicationSymbol];
                        if (conjunctAreTrueInModel && implicationSymbolIsFalseInModel) holds = false;
                    }

                    return holds;
                });

                var result = doesKbHoldInModel ? model.SentenceToTruthValue[query] : true;
               
               if (doesKbHoldInModel && model.SentenceToTruthValue[query]) _entailedCount++;
               
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

            var trueQuery = TruthTableQueryRecursive(kb, query, symbols, trueAssignedModel);
            var falseQuery = TruthTableQueryRecursive(kb, query, symbols, falseAssignedModel);
            return trueQuery
                   && falseQuery;
        }

        /*private TruthTableModel evaluateComplexSentences(HornFormKnowledgeBase inputKb, TruthTableModel inputModel)
        {
            
        }*/
    }
}