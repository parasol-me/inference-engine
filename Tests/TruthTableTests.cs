using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace assignment2.Tests
{
    public class TruthTableTests
    {
        private TruthTable tt = new();
        private HornFormKnowledgeBase _kb;
        
        [SetUp]
        public void Init()
        {
            var hc1 = new HornClause(null, true, new HashSet<string> {"a"});
            var hc2 = new HornClause(null, true, new HashSet<string> {"b"});
            var hc3 = new HornClause(null, true, new HashSet<string> {"c"});
            var listHc = new Dictionary<string, HornClause>(){{"a", hc1}, {"b", hc2}, {"c", hc3}};
            _kb = new HornFormKnowledgeBase(listHc);
        }

        [Test]
        public void TestTt()
        {
            var queryResult = tt.DoesEntail(_kb, "d");
            var queryResult2 = tt.DoesEntail(_kb, "a");
            var queryResult3 = tt.DoesEntail(_kb, "b");
            var queryResult4 = tt.DoesEntail(_kb, "c");
            Assert.IsFalse(queryResult.Result);
            Assert.IsTrue(queryResult2.Result && queryResult3.Result && queryResult4.Result);
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