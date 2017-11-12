using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BatchMaster
{
    public static class BatchMaster
    {
        public static ExecutionResult Execute<T>(IEnumerable<T> inputList, Action<IEnumerable<T>> actionToApply, ExecutionConfig execConfig)
        {
            ExecutionResult execResult = null;

            if (inputList != null && inputList.Any() && actionToApply != null && execConfig != null)
            {
                var generalStopWatch = new Stopwatch();

                generalStopWatch.Start();

                int totalCount = inputList.Count();
                int batchesCount = (int)Math.Ceiling(totalCount / (double)execConfig.BatchSize);

                int firstBatchCount = 0;
                int lastBatchCount = 0;

                Dictionary<int, string> failedBatches = new Dictionary<int, string>();

                for (int i = 0; i < batchesCount; i++)
                {
                    try
                    {
                        var batchData = inputList.Skip(i * execConfig.BatchSize).Take(execConfig.BatchSize);
                        actionToApply(batchData);

                        if (i == 0)
                        {
                            firstBatchCount = batchData.Count();
                        }

                        if (i == (batchesCount - 1))
                        {
                            lastBatchCount = batchData.Count();
                        }

                        if (i != (batchesCount - 1) && execConfig.BatchDelayInMS > 0)
                        {
                            Thread.Sleep(execConfig.BatchDelayInMS);
                        }
                    }
                    catch (Exception ex)
                    {
                        failedBatches.Add(i, ex.Message);
                    }
                }

                generalStopWatch.Stop();

                var totalDelays = new TimeSpan(0, 0, 0, 0, execConfig.BatchDelayInMS * (batchesCount - 1));
                var elapsedTimeWithoutDelays = generalStopWatch.Elapsed.Subtract(totalDelays);

                execResult = new ExecutionResult
                {
                    TotalCount = totalCount,
                    BatchesCount = batchesCount,
                    FirstBatchCount = firstBatchCount,
                    LastBatchCount = lastBatchCount,
                    TotalExecutionTime = elapsedTimeWithoutDelays,
                    AverageBatchExecutionTime = new TimeSpan(elapsedTimeWithoutDelays.Ticks / batchesCount),
                    TotalDelays = totalDelays,
                    FailedBatches = failedBatches
                };
            }

            return execResult;
        }

        public static ExecutionResult ExecuteParallel<T>(IEnumerable<T> inputList, Action<IEnumerable<T>> actionToApply, ExecutionConfig execConfig, ParallelOptions parallelOptions = null)
        {
            ExecutionResult execResult = null;

            if (inputList != null && inputList.Any() && actionToApply != null && execConfig != null)
            {
                var generalStopWatch = new Stopwatch();

                generalStopWatch.Start();

                int totalCount = inputList.Count();
                int batchesCount = (int)Math.Ceiling(totalCount / (double)execConfig.BatchSize);

                int firstBatchCount = 0;
                int lastBatchCount = 0;

                parallelOptions = parallelOptions ?? new ParallelOptions();

                Dictionary<int, string> failedBatches = new Dictionary<int, string>();

                Parallel.For(0, batchesCount, parallelOptions, (i) =>
                {
                    try
                    {
                        var batchData = inputList.Skip(i * execConfig.BatchSize).Take(execConfig.BatchSize);
                        actionToApply(batchData);

                        if (i == 0)
                        {
                            firstBatchCount = batchData.Count();
                        }

                        if (i == (batchesCount - 1))
                        {
                            lastBatchCount = batchData.Count();
                        }

                        parallelOptions.CancellationToken.ThrowIfCancellationRequested();
                    }
                    catch (Exception ex)
                    {
                        failedBatches.Add(i, ex.Message);
                    }
                });

                generalStopWatch.Stop();

                execResult = new ExecutionResult
                {
                    TotalCount = totalCount,
                    BatchesCount = batchesCount,
                    FirstBatchCount = firstBatchCount,
                    LastBatchCount = lastBatchCount,
                    TotalExecutionTime = generalStopWatch.Elapsed,
                    AverageBatchExecutionTime = new TimeSpan(generalStopWatch.Elapsed.Ticks / batchesCount),
                    FailedBatches = failedBatches
                };
            }

            return execResult;
        }
    }
}
