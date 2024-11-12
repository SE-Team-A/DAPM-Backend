using RabbitMQLibrary.Models;

/// <author>Nicolai Veiglin Arends</author>
/// <author>Tamás Drabos</author>
namespace DAPM.ClientApi.Models.DTOs
{
    public class PipelineExecutionApiDto
    {
        public string Name { get; set; }
        public PipelineExecution PipelineExecution { get; set; }
    }
}
