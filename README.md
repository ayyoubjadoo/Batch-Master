# Batch-Master

A light library which contains useful helpers for batching any process (executing tasks in batches)

## Sample Usage


            List<int> myIntList = new List<int>();

            for (int i = 0; i < 100; i++)
            {
                myIntList.Add(i + 1);
            }

            var config = new ExecutionConfig
            {
                BatchSize = 30,
                BatchDelayInMS = 200 //this property will be ignored in the Parallel mode
            };

            Console.WriteLine("Normal:");
            var result = BatchMaster.Execute(myIntList, (batch) => { DoSomethingWithMyBatch(batch.ToList()); }, config);
            Console.WriteLine(result);

            Console.WriteLine("-------------------------------------");

            Console.WriteLine("Parallel:");
            var pResult = BatchMaster.ExecuteParallel(myIntList, (batch) => { DoSomethingWithMyBatch(batch.ToList()); }, config);
            Console.WriteLine(pResult);

            Console.ReadLine();

### Made with love by Ayyoub Jadoo
