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
            var listHC = new Dictionary<string, HornClause>(){{"a", hc1}, {"b", hc2}, {"c", hc3}};
            kb = new HornFormKnowledgeBase(listHC);
        }

        [Test]
        public void testTT()
        {
            Assert.IsFalse(tt.DoesEntail(kb, "d").Result);
            Assert.IsTrue(tt.DoesEntail(kb, "a").Result && tt.DoesEntail(kb, "b").Result && tt.DoesEntail(kb, "c").Result);
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