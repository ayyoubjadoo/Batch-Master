# Batch-Master
By Ayyoub Jadoo

A light library which contains useful helpers for batching any process (executing tasks in batches)

## Sample Usage

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchMaster.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> myIntList = new List<int>();

            for (int i = 100; i > -5; i--)
            {
                myIntList.Add(i + 1);
            }

            var config = new ExecutionConfig {
                BatchSize = 5,
                BatchDelayInMS = 0
            };

            Console.WriteLine("Normal:");
            ### var result = BatchMaster.Execute(myIntList, (batch) => { ProcessMyList(batch.ToList()); }, config);
            Console.WriteLine(result);

            Console.WriteLine("-------------------------------------");

            Console.WriteLine("Parallel:");
            ### var pResult = BatchMaster.ExecuteParallel(myIntList, (batch) => { ProcessMyList(batch.ToList()); }, config);
            Console.WriteLine(pResult);

            Console.ReadLine();
        }

        private static void ProcessMyList(List<int> list)
        {
            list.ForEach(i => { int x = 100 / i; });
        }
    }
}
