using System.Collections.Generic;
using NUnit.Framework;

namespace assignment2.Tests
{
    public class TruthTableTests
    {
        private TruthTable tt = new TruthTable();
        private HornFormKnowledgeBase kb;

        // [SetUp]
        // public void init()
        // {
        //     HornClause hc1 = new HornClause(null, true, new HashSet<string> {"a"});
        //     HornClause hc2 = new HornClause(null, true, new HashSet<string> {"b"});
        //     HornClause hc3 = new HornClause(null, true, new HashSet<string> {"c"});
        //     List<HornClause> listHC = new List<HornClause>(){hc1, hc2, hc3};
        //     kb = new HornFormKnowledgeBase(listHC);
        // }
        //
        //
        // [Test]
        // public void testOne()
        // {
        //     HashSet<string> result = tt.DoesEntail(kb, "d");
        //     Assert.AreEqual(4, result.Count);
        // }
        //
        // [Test]
        // public void NumbersToBinary()
        // {
        //     HashSet<string> result = tt.NumToBits(3);
        //     Assert.AreEqual(3, result.Count);
        // }
        //
        // [Test]
        // public void CreateTable()
        // {
        //     tt.CreateTable(tt.DoesEntail(kb, "a"));
        // }
    }
}