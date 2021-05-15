using System.Collections.Generic;
using NUnit.Framework;

namespace assignment2.Tests
{
    public class TruthTableTests
    {
        private TruthTable tt = new TruthTable();
        private HornFormKnowledgeBase kb;
        
        [SetUp]
        public void init()
        {
            var hc1 = new HornClause(null, true, new HashSet<string> {"a"});
            var hc2 = new HornClause(null, true, new HashSet<string> {"b"});
            var hc3 = new HornClause(null, true, new HashSet<string> {"c"});
            var listHC = new Dictionary<string, HornClause>() { };
            kb = new HornFormKnowledgeBase(listHC);
        }

        [Test]
        public void testTT()
        {
            Assert.IsFalse(tt.DoesEntail(kb, "d"));
            Assert.IsTrue(tt.DoesEntail(kb, "a") && tt.DoesEntail(kb, "b") && tt.DoesEntail(kb, "c"));
        }
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