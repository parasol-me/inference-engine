using System;
using System.Collections.Generic;
using System.Linq;
using assignment2.enums;

namespace assignment2
{
    public class InferenceEngine
    {
        public QueryResult RunQueryForHornForm(HornFormKnowledgeBase knowledgeBase, string querySymbol, AlgorithmType algorithmType)
        {
            switch (algorithmType)
            {
                case AlgorithmType.Tt:
                    throw new NotImplementedException();
                case AlgorithmType.Fc:
                    return ForwardChainingQueryRecursive(knowledgeBase, querySymbol);
                case AlgorithmType.Bc:
                    return BackwardChainingQueryRecursive(knowledgeBase, querySymbol
                    , new HashSet<string>(), new HashSet<string>(), new HashSet<string>());
                default:
                    throw new ArgumentOutOfRangeException(nameof(algorithmType), algorithmType, null);
            }
        }

        private QueryResult ForwardChainingQueryRecursive(HornFormKnowledgeBase knowledgeBase, string querySymbol)
        {
            var agenda = new List<HornClause>();     //symbols which are not completely implied (awaiting conjunct symbol)
            var inferred = new HashSet<string>();   //symbols proven true
            var queried = new HashSet<string>();    //don't get check again
            var queue = knowledgeBase.Clauses.Where(clause => clause.FinalImplication == true).ToList(); //initialised with Facts
            //couldnt get the LINQ wizardry working here, basic foreach.. 
            foreach (var clause in queue)
            {
                inferred.Add(clause.ConjunctSymbols.First()); //always first, as assessing fact (singular conjunct symbol)
            }

            knowledgeBase.Clauses.RemoveAll(clause => clause.FinalImplication == true); //stops assessing fact against itself
            
            while (queue.Count > 0)
            {
                var clauseUnderAssessment = queue.First();
                queried.Add(clauseUnderAssessment.ConjunctSymbols.First());
                queue.Remove(clauseUnderAssessment);
                //add to another list (checked) instead?

                foreach (var kbClause in knowledgeBase.Clauses)
                {
                    if (kbClause.ConjunctSymbols.Contains(clauseUnderAssessment.ConjunctSymbols.First())) //always first, as assessing fact (singular symbol)
                    {
                        if (kbClause.ConjunctSymbols.Count.Equals(1) && kbClause.ImplicationSymbol != null)
                        {
                            //clauseUnderAssessment solely implies another symbol (no conjunction)
                            inferred.Add(kbClause.ImplicationSymbol);
                            //if inferred symbol is query, break early, else add to queue
                            if (kbClause.IsSymbolFact(querySymbol))
                                return new QueryResult(true, null, null, null); //todo
                            queue.Add(new HornClause(null, true, new HashSet<string>(){kbClause.ImplicationSymbol}));
                        }
                        else
                        {
                            //if all conjunct symbols are proven, we can prove the Implication
                            if (isClauseImplied(kbClause, inferred))
                                queue.Add(new HornClause(null, true, new HashSet<string>() {kbClause.ImplicationSymbol}));
                            //if any one isn't, add to agenda
                            else 
                                agenda.Add(kbClause);
                        }
                    }
                }
                //Check agenda, if all conjunct symbols of any clause now prove implication, add to queue
                foreach (var agendaItem in agenda)
                {
                    if (isClauseImplied(agendaItem, inferred))
                        queue.Add(new HornClause(null, true, new HashSet<string>() {agendaItem.ImplicationSymbol}));
                }
            }
            return new QueryResult(false, null, null, null);
            //todo
        }

        private bool isClauseImplied(HornClause agendaItem, HashSet<string> inferred)
        {
            bool allSymbolsTrue = true;
            foreach (var symbol in agendaItem.ConjunctSymbols)
            {
                if (!inferred.Contains(symbol))
                {
                    allSymbolsTrue = false;
                    break;
                }
            }
            return allSymbolsTrue;
        }

        private QueryResult BackwardChainingQueryRecursive(HornFormKnowledgeBase knowledgeBase, string querySymbol, 
            HashSet<string> existingEntailed, HashSet<string> existingQueried, HashSet<string> existingProvedFalse)
        {
            // keep track of symbols proved true, symbols already queried and symbols proved false from previous recursions
            var entailed = new HashSet<string>(existingEntailed);
            var queried = new HashSet<string>(existingQueried);
            var provedFalse = new HashSet<string>(existingProvedFalse);
            
            // find clauses that directly relates to the query symbol
            var queryClauses = knowledgeBase.Clauses.Where(clause => clause.ImplicationSymbol == querySymbol ||
                                                                     clause.IsSymbolFact(querySymbol)
            ).ToList();

            // no symbol was found that implies the query symbol or proves it as fact
            if (!queryClauses.Any()) return new QueryResult(false, entailed, queried, provedFalse);
            
            // query was proven as fact in KB already, no need to continue
            if (queryClauses.Any(clause => clause.IsSymbolFact(querySymbol)))
            {
                queryClauses.SelectMany(clause => clause.ConjunctSymbols).ToList().ForEach(symbol => entailed.Add(symbol));
                return new QueryResult(true, entailed, queried, provedFalse);
            }

            foreach (var queryClause in queryClauses)
            {
                foreach (var symbol in queryClause.ConjunctSymbols)
                {
                    // if we already queried this symbol, or it had already been proved true, skip
                    if (queried.Contains(symbol) || entailed.Contains(symbol)) continue;
                    
                    // if one of the conjunct symbols had been proved false already, the queried symbol is also false
                    // stop all work and return false
                    if (provedFalse.Contains(symbol))
                        return new QueryResult(false, entailed, queried, provedFalse);
                    
                    // add symbol to already queried just before querying so that recursive functions can see we already started this path
                    queried.Add(symbol);
                    
                    // query symbol recursively, add entailed symbols from recursive result
                    var queryResult = BackwardChainingQueryRecursive(knowledgeBase, symbol, entailed, queried, provedFalse);
                    entailed.UnionWith(queryResult.Entailed);
                    
                    // add all queried and proved false symbols from recursive result
                    queried.UnionWith(queryResult.Queried);
                    provedFalse.UnionWith(queryResult.ProvedFalse);

                    // add to set if symbol proved false
                    if (!queryResult.Result) provedFalse.Add(symbol);
                }

                // if all conjunct symbols are true, query symbol must be true, stop all work and return as entailed
                if (queryClause.ConjunctSymbols.IsSubsetOf(entailed))
                {
                    entailed.Add(querySymbol);
                    break;
                }
            }

            // if query was proved by entailment, return true or return false along with anything we proved along the way
            return entailed.Contains(querySymbol) ? new QueryResult(true, entailed, queried, provedFalse) : new QueryResult(false, entailed, queried, provedFalse);
        }
    }
}