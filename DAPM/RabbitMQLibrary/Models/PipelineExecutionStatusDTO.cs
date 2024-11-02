using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Models
{
    public class PipelineExecutionStatusDTO
    {
        public TimeSpan ExecutionTime { get; set; }
        public List<StepStatusDTO> CurrentSteps { get; set; }
        public string State { get; set; }
    }

    public class PipelineExecution
    {
        public Guid ExecutionId { get; set; }
        public Guid PipelineId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string State { get; set; }
    }
}

