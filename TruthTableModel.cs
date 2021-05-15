using System.Collections.Generic;

namespace assignment2
{
    public class TruthTableModel
    {
        public TruthTableModel(Dictionary<string, bool?> sentenceToTruthValue)
        {
            SentenceToTruthValue = sentenceToTruthValue;
        }

        public Dictionary<string, bool?> SentenceToTruthValue { get; }
    }
}