using System;
using System.Collections.Generic;
using System.Linq;

namespace assignment2
{
    public class TruthTable
    {
        private int _entailedCount;
        private int _totalNumberOfModels;

        //figure 7.10
        public QueryResult DoesEntail(HornFormKnowledgeBase kb, string query)
        {
            _entailedCount = 0;
            _totalNumberOfModels = 0;
            var symbols = kb.Clauses.SelectMany(clause => 
                    new HashSet<string>(clause.Value.ConjunctSymbols){clause.Value.ImplicationSymbol, query})
                .Where(symbol => symbol != null)
                .ToHashSet();

            return new QueryResult(
                TruthTableQueryRecursive(kb, query, symbols, new TruthTableModel(new Dictionary<string, bool>())),
                new HashSet<string>() {_entailedCount.ToString()}, new HashSet<string>() {_totalNumberOfModels.ToString()}, null);
        }

        //figure 7.10
        private bool TruthTableQueryRecursive(HornFormKnowledgeBase kb, string query, HashSet<string> symbols, TruthTableModel model)
        {
            if (symbols.Count == 0)
            {
                _totalNumberOfModels++;
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

            var firstSymbol = symbols.First();
            var rest = symbols.Where(symbol => symbol != firstSymbol).ToHashSet();

            //recursively assign true/false values to all symbols, filling model
            var trueAssignedModel = 
                new TruthTableModel(new Dictionary<string, bool>(model.SentenceToTruthValue)
                {
                    {firstSymbol, true}
                });
            
            var falseAssignedModel = 
                new TruthTableModel(new Dictionary<string, bool>(model.SentenceToTruthValue)
                {
                    {firstSymbol, false}
                });

            return TruthTableQueryRecursive(kb, query, rest, trueAssignedModel)
                   && TruthTableQueryRecursive(kb, query, rest, falseAssignedModel);
        }

        /*private TruthTableModel evaluateComplexSentences(HornFormKnowledgeBase inputKb, TruthTableModel inputModel)
        {
            
        }*/
    }
}