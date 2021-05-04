using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var isMethodValid = Enum.TryParse<AlgorithmType>(args[0], true, out var algorithmType);
            if (!isMethodValid)
            {
                throw new Exception($"The input algorithm type ({algorithmType}) is not valid." +
                                    $"\nValid inputs are {AlgorithmType.Tt}, {AlgorithmType.Fc}, {AlgorithmType.Bc}.");
            }

            var knowledgeBase = ReadKnowledgeBaseFromFile(fileName);
            var query = ReadQueryFromFile(fileName);
            
            var inferenceEngine = new InferenceEngine();
            
            var watch = new Stopwatch();
            
            watch.Start();

            var queryResult = inferenceEngine.RunQuery(knowledgeBase, query, algorithmType);
            
            watch.Stop();
            
            Console.WriteLine($"{(queryResult.Result ? "YES" : "NO")}: {string.Join(", ", queryResult.Entailed)}");
        }

        private static KnowledgeBase ReadKnowledgeBaseFromFile(string fileName)
        {
            throw new NotImplementedException();
        }
        
        private static string ReadQueryFromFile(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}