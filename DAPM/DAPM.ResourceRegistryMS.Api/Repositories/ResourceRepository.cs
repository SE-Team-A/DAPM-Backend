using Microsoft.EntityFrameworkCore;
using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Repositories.Interfaces;
/// <author>Ayat Al Rifai</author>

namespace DAPM.ResourceRegistryMS.Api.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly ResourceRegistryDbContext _context;
        private ILogger<ResourceRepository> _logger;

        public ResourceRepository(ResourceRegistryDbContext context, ILogger<ResourceRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Resource>> GetAllResources()
        {
            return await _context.Resources.ToListAsync();
        }

        public async Task<bool> AddResource(Resource resource)
        {
            await _context.Resources.AddAsync(resource);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteResource(Guid organizationId, Guid repositoryId, Guid resourceId)
        {
            //var resource = await _context.Resources.FindAsync(resourceId);
            var found = _context.Resources.Where(r => r.PeerId == organizationId && r.RepositoryId == repositoryId && r.Id == resourceId).FirstOrDefault();

            if (found == null)
            {
                _logger.LogInformation("found is null ");            
                    return false;
            }

            _context.Resources.Remove(found);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Resource> GetResourceById(Guid organizationId, Guid repositoryId, Guid resourceId)
        {
            return (Resource)_context.Resources.Where(r => r.PeerId == organizationId && r.RepositoryId == repositoryId && r.Id == resourceId);
        }

        public IEnumerable<Resource> GetResourcesOfRepository(Guid organizationId, Guid repositoryId)
        {
            return _context.Resources.Where(r => r.PeerId == organizationId && r.RepositoryId == repositoryId);
        }

    }
}
