using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchMaster
{
    public class ExecutionResult
    {
        public int BatchesCount { get; set; }

        public int TotalCount { get; set; }

        public int FirstBatchCount { get; set; }

        public int LastBatchCount { get; set; }

        public TimeSpan TotalExecutionTime { get; set; }

        public TimeSpan AverageBatchExecutionTime { get; set; }

        public TimeSpan TotalDelays { get; set; }

        public Dictionary<int, string> FailedBatches { get; set; }

        public override string ToString()
        {
            return $"Execution Result:{Environment.NewLine}Total Count: {TotalCount}{Environment.NewLine}Batches Count: {BatchesCount}{Environment.NewLine}First Batch Count: {FirstBatchCount}{Environment.NewLine}Last Batch Count: {LastBatchCount}{Environment.NewLine}Total Execution Time: {TotalExecutionTime}{Environment.NewLine}Batch Average Execution Time: {AverageBatchExecutionTime}{Environment.NewLine}Total Delays Time: {TotalDelays}{Environment.NewLine}";
        }
    }
}
