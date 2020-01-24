using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChallengeNewRelicV3
{
    class NewRelicCHallenge
    {
        public  static async Task Main(string[] args)
        {
            Dictionary<string, int> sequences = new Dictionary<string, int>();
            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            try
            {
                if (args.Length > 0)
                {
                    foreach (var file in args)
                    {
                        using (var str = File.OpenText(dir + @"\\" + file.ToString()))
                        {
                            sequences = await ProcessStream(str, sequences);
                        }
                    }
                }
                else
                {
                    using (var stdStream = Console.In)
                    {
                        sequences = await ProcessStream(stdStream, sequences);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            foreach (var sequence in sequences.OrderByDescending(x => x.Value).Take(100))
            {
                Console.WriteLine($"{sequence.Key} - {sequence.Value}");
            }
        }

        private static async Task<Dictionary<string, int>> ProcessStream(TextReader str, Dictionary<string, int> sequences)
        {
            string currentLine = string.Empty;
            List<string> currentLineList = new List<string>();
            Queue<string> currentSequenceQueue = new Queue<string>();
            //Dictionary<string, int> sequences = new Dictionary<string, int>();
            while ((currentLine = str.ReadLine()) != null)
            {
                currentLineList = Regex.Replace(currentLine, @"[^A-Za-z0-9]+", " ")
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var word in currentLineList)
                {
                    currentSequenceQueue.Enqueue(word);
                    if (currentSequenceQueue.Count < 3)
                    {
                        continue;
                    }
                    string currentSequence = String.Join(" ", currentSequenceQueue).ToLower();
                    if (!sequences.ContainsKey(currentSequence))
                    {
                        sequences[currentSequence] = 1;
                    }
                    else
                    {
                        sequences[currentSequence]++;
                    }
                    currentSequenceQueue.Dequeue();
                }
            }
            return sequences;
        }

    }
}