using System.Collections.Generic;

namespace assignment2
{
    public class HornFormKnowledgeBase
    {
        public HornFormKnowledgeBase(List<HornClause> clauses)
        {
            Clauses = clauses;
        }

        public List<HornClause> Clauses { get; }
    }

}