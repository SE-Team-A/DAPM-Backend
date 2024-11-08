using RabbitMQLibrary.Models;

namespace DAPM.ClientApi.Models.DTOs
{
    public class PipelineExecutionApiDto
    {
        public string Name { get; set; }
        public PipelineExecution PipelineExecution { get; set; }
    }
}
