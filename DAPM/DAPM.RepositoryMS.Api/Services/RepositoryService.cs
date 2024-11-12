using DAPM.RepositoryMS.Api.Models;
using DAPM.RepositoryMS.Api.Models.MongoDB;
using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using DAPM.RepositoryMS.Api.Services.Interfaces;
using MongoDB.Bson;
using RabbitMQLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <author>Nicolai Veiglin Arends</author>
/// <author>Tamás Drabos</author>
namespace DAPM.RepositoryMS.Api.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly ILogger<RepositoryService> _logger;
        private readonly IResourceRepository _resourceRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IRepositoryRepository _repositoryRepository;
        private readonly IPipelineRepository _pipelineRepository;
        private readonly IOperatorRepository _operatorRepository;

        public RepositoryService(ILogger<RepositoryService> logger,
            IResourceRepository resourceRepository,
            IFileRepository fileRepository,
            IRepositoryRepository repositoryRepository,
            IPipelineRepository pipelineRepository,
            IOperatorRepository operatorRepository)
        {
            _logger = logger;
            _resourceRepository = resourceRepository;
            _fileRepository = fileRepository;
            _repositoryRepository = repositoryRepository;
            _pipelineRepository = pipelineRepository;
            _operatorRepository = operatorRepository;
        }

        public async Task<bool> DeleteResource(Guid organisationId,Guid repositoryId, Guid resourceId){

              _logger.LogInformation($"Deleting resource with ID: {resourceId} from repository ID: {repositoryId}");

            var resource = await _resourceRepository.GetResourceById(repositoryId,resourceId);
            if (resource == null)
            {
                _logger.LogWarning($"Resource with ID: {resourceId} not found in repository ID: {repositoryId}");
                return false;
            }

            // Optionally delete the associated file from MongoDB
          /*  if (resource.File != null && !string.IsNullOrEmpty(resource.File.MongoDbFileId))
            {
                bool fileDeleted = await _fileRepository.DeleteFile(resource.File.MongoDbFileId);
                if (!fileDeleted)
                {
                    _logger.LogError($"Failed to delete the file with MongoDb ID: {resource.File.MongoDbFileId}");
                    return false;
                }
            }*/

            // Delete the resource from PostgreSQL
            bool resourceDeleted = await _resourceRepository.DeleteResource(organisationId,repositoryId, resourceId);
            if (!resourceDeleted)
            {
                _logger.LogError($"Failed to delete the resource with ID: {resourceId}");
                return false;
            }

            _logger.LogInformation($"Successfully deleted resource with ID: {resourceId} from repository ID: {repositoryId}");
            return true;
        }

        public async Task<Models.PostgreSQL.Pipeline> CreateNewPipeline(Guid repositoryId, string name, RabbitMQLibrary.Models.Pipeline pipeline)
        {
            var pipelineJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(pipeline);

            var pipelineObject = new Models.PostgreSQL.Pipeline
            {
                Name = name,
                RepositoryId = repositoryId,
                PipelineJson = pipelineJsonString
            };

            var createdPipeline = await _pipelineRepository.AddPipeline(pipelineObject);

            return createdPipeline;
        }

        public async Task<Models.PostgreSQL.PipelineExecution> CreateNewPipelineExecution(Guid repositoryId, string name, RabbitMQLibrary.Models.PipelineExecution pipelineExecution)
        {
            var pipelineExecutionJson = Newtonsoft.Json.JsonConvert.SerializeObject(pipelineExecution);

            var pipelineExecutionObject = new Models.PostgreSQL.PipelineExecution
            {
                Name = name,
                RepositoryId = repositoryId,
                PipelineExecutionJson = pipelineExecutionJson
            };

            var createdPipelineExecution = await _pipelineRepository.AddPipelineExecution(pipelineExecutionObject);

            return createdPipelineExecution;
        }

        public async Task<Models.PostgreSQL.Resource> CreateNewResource(Guid repositoryId, string name, string resourceType, FileDTO fileDto)
        {
            _logger.LogInformation($"THE REPO ID IS {repositoryId}");
            var repository = await _repositoryRepository.GetRepositoryById(repositoryId);

            if (repository != null)
            {
                string objectId = await _fileRepository.AddFile(new MongoFile { Name = fileDto.Name, File = fileDto.Content });

                if (objectId != null)
                {
                    var file = new Models.PostgreSQL.File
                    {
                        Name = fileDto.Name,
                        MongoDbFileId = objectId,
                        Extension = fileDto.Extension
                    };

                    var resource = new Models.PostgreSQL.Resource
                    {
                        Name = name,
                        File = file,
                        Repository = repository,
                        Type = resourceType
                    };

                    var newResource = await _resourceRepository.AddResource(resource);

                    return newResource;
                }
            }

            return null;
        }

        public async Task<Models.PostgreSQL.Operator> CreateNewOperator(Guid repositoryId, string name, string resourceType, FileDTO sourceCode, FileDTO dockerfile)
        {
            var repository = await _repositoryRepository.GetRepositoryById(repositoryId);

            if (repository != null)
            {
                string sourceCodeObjectId = await _fileRepository.AddFile(new MongoFile { Name = sourceCode.Name, File = sourceCode.Content });
                string dockerfileObjectId = await _fileRepository.AddFile(new MongoFile { Name = dockerfile.Name, File = dockerfile.Content });

                if (sourceCodeObjectId != null && dockerfileObjectId != null)
                {
                    var sourceCodeFile = new Models.PostgreSQL.File
                    {
                        Name = sourceCode.Name,
                        MongoDbFileId = sourceCodeObjectId,
                        Extension = sourceCode.Extension
                    };

                    var dockerfileFile = new Models.PostgreSQL.File
                    {
                        Name = dockerfile.Name,
                        MongoDbFileId = dockerfileObjectId,
                        Extension = dockerfile.Extension
                    };

                    var op = new Models.PostgreSQL.Operator
                    {
                        Name = name,
                        Repository = repository,
                        Type = resourceType,
                        DockerfileFile = dockerfileFile,
                        SourceCodeFile = sourceCodeFile,
                    };

                    var newOperator = await _operatorRepository.AddOperator(op);

                    return newOperator;
                }
            }

            return null;
        }

        public async Task<Repository> CreateNewRepository(string name)
        {
            return await _repositoryRepository.CreateRepository(name);
        }

        public Task<IEnumerable<Models.PostgreSQL.Pipeline>> GetPipelinesFromRepository(Guid repositoryId)
        {
            throw new NotImplementedException();
        }

    }  
}
