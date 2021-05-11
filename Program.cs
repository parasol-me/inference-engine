using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using assignment2.enums;

namespace assignment2
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2 || args[0] == null || args[1] == null || args[0].Length == 0 || args[1].Length == 0)
            {
                throw new Exception($"Invalid number of arguments." +
                                    $"\nPlease run the program with the following format:" +
                                    $"\niengine <method> <filename>");
            }
            var fileName = args[1];
            // var isMethodValid = Enum.TryParse<AlgorithmType>(args[0], true, out var algorithmType);
            // if (!isMethodValid)
            // {
            //     throw new Exception($"The input algorithm type ({algorithmType}) is not valid." +
            //                         $"\nValid inputs are {AlgorithmType.Tt}, {AlgorithmType.Fc}, {AlgorithmType.Bc}.");
            // }

            var algorithmType = AlgorithmType.Bc;

            var knowledgeBase = ReadKnowledgeBaseFromFile(fileName);
            var query = ReadQueryFromFile(fileName);
            var inferenceEngine = new InferenceEngine();
            var watch = new Stopwatch();
            
            watch.Start();

            var queryResult = inferenceEngine.RunQueryForHornForm(knowledgeBase, query, algorithmType);
            
            watch.Stop();
            
            Console.WriteLine($"{(queryResult.Result ? "YES" : "NO")}: {string.Join(", ", queryResult.Entailed)}");
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }

        private static HornFormKnowledgeBase ReadKnowledgeBaseFromFile(string fileName)
        {
            var file = new System.IO.StreamReader($@"{fileName}");

            // find tell line
            string line = file.ReadLine();
            while (line != null && line.ToLower() != "tell")
            {
                line = file.ReadLine();
                //todo ?
            }
            if (line == null || line.ToLower() != "tell") throw new Exception("file is not valid");
            
            // KB follows tell line
            line = file.ReadLine();
            var clauses = new List<HornClause>();
            if (line == null) throw new Exception("file is not valid");
            var clauseRegex = new Regex(@"([^;]+)");
            var clauseMatches = clauseRegex.Matches(line);
            foreach (Match clause in clauseMatches)
            {
                // for each matched clause, get the conjunct symbols and the implication symbols or final implication
                var splitClause = clause.Value.Split("=>");
                var symbols = splitClause[0];
                var implicationString = splitClause.Length == 2 ? splitClause[1] : null;
                //todo multiple conjunctions?
                var finalImplication =
                    implicationString == null || implicationString.ToLower() == "true"
                        ? true
                        : implicationString.ToLower() == "false"
                            ? false
                            : (bool?) null;
                var implicationSymbol = finalImplication == null ? implicationString.Trim() : null;
                var conjunctSymbolsRegex = new Regex(@"([^&]+)");
                var conjunctSymbolsMatches = conjunctSymbolsRegex.Matches(symbols);
                clauses.Add(new HornClause(implicationSymbol, finalImplication, conjunctSymbolsMatches.Select(match => match.Value.Trim()).ToHashSet()));
            }

            return new HornFormKnowledgeBase(clauses);
        }
        
        private static string ReadQueryFromFile(string fileName)
        {
            var file = new System.IO.StreamReader($@"{fileName}");

            // find ask line
            string line = file.ReadLine();
            while (line != null && line.ToLower() != "ask")
            {
                line = file.ReadLine();
            }
            if (line == null || line.ToLower() != "ask") throw new Exception("file is not valid");
            
            // query follows ask line
            line = file.ReadLine();
            if (line == null) throw new Exception("file is not valid");
            return line.Trim();
        }
    }
}