using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using RabbitMQLibrary.Models;

namespace DAPM.RepositoryMS.Api.Services.Interfaces
{
    public interface IRepositoryService
    {
        Task<Models.PostgreSQL.Resource> CreateNewResource(Guid repositoryId, string name, string resourceType, FileDTO file);
        Task<Models.PostgreSQL.Operator> CreateNewOperator(Guid repositoryId, string name, string resourceType, FileDTO sourceCode, FileDTO dockerfile);
        Task<Models.PostgreSQL.Pipeline> CreateNewPipeline(Guid repositoryId, string name, RabbitMQLibrary.Models.Pipeline pipeline);
        Task<Repository> CreateNewRepository(string name);
        Task<IEnumerable<Models.PostgreSQL.Pipeline>> GetPipelinesFromRepository(Guid repositoryId);
        Task<bool> DeleteResource(Guid organizationId, Guid repositoryId, Guid resourceId);
        Task<Models.PostgreSQL.Pipeline> EditPipeline(Guid repositoryId, string name, RabbitMQLibrary.Models.Pipeline pipeline, Guid pipelineId);

    }
}
