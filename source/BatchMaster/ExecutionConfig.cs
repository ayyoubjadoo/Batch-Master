using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchMaster
{
    public class ExecutionConfig
    {
        public ExecutionConfig()
        {
            BatchSize = 10;
            BatchDelayInMS = 0;
        }

        public int BatchSize { get; set; }

        public int BatchDelayInMS { get; set; }
    }
}
