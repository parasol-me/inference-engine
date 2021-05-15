using System.Collections.Generic;

namespace assignment2
{
    public class HornFormKnowledgeBase
    {
        public HornFormKnowledgeBase(Dictionary<string, HornClause> clauses)
        {
            Clauses = clauses;
        }

        public Dictionary<string, HornClause> Clauses { get; }
    }
}