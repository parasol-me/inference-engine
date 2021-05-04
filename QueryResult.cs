using System;
using System.Collections.Generic;

namespace assignment2
{
    public class QueryResult
    {
        public bool Result { get; }
        
        public List<string> Entailed { get; }
        
        public QueryResult(bool result, List<string> entailed)
        {
            Result = result;
            Entailed = entailed;
        }
    }
}