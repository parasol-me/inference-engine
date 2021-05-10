using System;
using System.Collections.Generic;

namespace assignment2
{
    public class QueryResult
    {
        public bool Result { get; }
        
        public HashSet<string> Entailed { get; }
        
        public HashSet<string> ProvedFalse { get; }
        
        public HashSet<string> Queried { get; }
        
        public QueryResult(bool result, HashSet<string> entailed, HashSet<string> queried, HashSet<string> provedFalse)
        {
            Result = result;
            Entailed = entailed;
            Queried = queried;
            ProvedFalse = provedFalse;
        }
    }
}