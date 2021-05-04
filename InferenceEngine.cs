using System;
using assignment2.enums;

namespace assignment2
{
    public class InferenceEngine
    {
        public QueryResult RunQuery(KnowledgeBase knowledgeBase, string querySymbol, AlgorithmType method)
        {
            switch (method)
            {
                case AlgorithmType.Tt:
                    throw new NotImplementedException();
                case AlgorithmType.Fc:
                    throw new NotImplementedException();
                case AlgorithmType.Bc:
                    throw new NotImplementedException();;
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, null);
            }
        }
    }
}