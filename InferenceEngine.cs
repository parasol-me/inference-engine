﻿using System;
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
                    var tt = new TruthTable();
                    return tt.DoesEntail(knowledgeBase, querySymbol);
                case AlgorithmType.Fc:
                    return ForwardChainingQuery(knowledgeBase, querySymbol);
                case AlgorithmType.Bc:
                    return BackwardChainingQueryRecursive(knowledgeBase, querySymbol
                    , new HashSet<string>(), new HashSet<string>(), new HashSet<string>());
                default:
                    throw new ArgumentOutOfRangeException(nameof(algorithmType), algorithmType, null);
            }
        }

        private QueryResult ForwardChainingQuery(HornFormKnowledgeBase knowledgeBase, string querySymbol)
        {
            var allClauses = knowledgeBase.Clauses.Values;
            var agenda = new List<HornClause>();     //symbols which are not completely implied (awaiting conjunct symbol)
            var queue = new Queue<HornClause>(allClauses.Where(clause => clause.FinalImplication == true).ToList()); //initialised with Facts
            var inferred = queue.Select(clause => clause.ConjunctSymbols.First()).ToHashSet(); //symbols proven true

            var clausesToSearch = allClauses.Where(clause => clause.FinalImplication != true).ToList(); // non fact clauses
            
            while (queue.Count > 0)
            {
                foreach (var kbClause in clausesToSearch)
                {
                    if (IsClauseImplied(kbClause, inferred) && kbClause.ImplicationSymbol != null)
                    {
                        // add to queue if not yet inferred
                        if (!inferred.Contains(kbClause.ImplicationSymbol)) 
                            queue.Enqueue(HornClause.AsFact(kbClause.ImplicationSymbol));
                        inferred.Add(kbClause.ImplicationSymbol);
                        
                        // if the query was proven, return result
                        if (kbClause.ImplicationSymbol.Equals(querySymbol))
                            return new QueryResult(true, inferred, null, null);
                    }
                    else
                    {
                        agenda.Add(kbClause);
                    }
                }
                
                //Check agenda, if all conjunct symbols of any clause now prove implication, add to queue
                foreach (var agendaItem in agenda.Where(agendaItem => 
                    IsClauseImplied(agendaItem, inferred)))
                {
                    if (agendaItem.ImplicationSymbol == null) continue;
                    
                    // add to queue if not yet inferred
                    if (!inferred.Contains(agendaItem.ImplicationSymbol))
                        queue.Enqueue(HornClause.AsFact(agendaItem.ImplicationSymbol));
                    inferred.Add(agendaItem.ImplicationSymbol);
                    
                        
                    // if the query was proven, return result
                    if (agendaItem.ImplicationSymbol.Equals(querySymbol))
                        return new QueryResult(true, inferred, null, null);
                }
            }
            return new QueryResult(false, inferred, null, null);
        }

        private static bool IsClauseImplied(HornClause clause, HashSet<string> inferred)
        {
            return clause.ConjunctSymbols.All(inferred.Contains);
        }

        private static QueryResult BackwardChainingQueryRecursive(HornFormKnowledgeBase knowledgeBase, string querySymbol, 
            HashSet<string> existingEntailed, HashSet<string> existingQueried, HashSet<string> existingProvedFalse)
        {
            var allClauses = knowledgeBase.Clauses.Values;
            // keep track of symbols proved true, symbols already queried and symbols proved false from previous recursions
            var entailed = new HashSet<string>(existingEntailed);
            var queried = new HashSet<string>(existingQueried);
            var provedFalse = new HashSet<string>(existingProvedFalse);
            
            // find clauses that directly relates to the query symbol
            var queryClauses = allClauses.Where(clause => clause.ImplicationSymbol == querySymbol ||
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