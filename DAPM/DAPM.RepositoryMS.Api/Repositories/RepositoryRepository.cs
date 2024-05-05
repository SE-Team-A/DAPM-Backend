﻿using DAPM.RepositoryMS.Api.Data;
using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAPM.RepositoryMS.Api.Repositories
{
    public class RepositoryRepository : IRepositoryRepository
    {
        private readonly RepositoryDbContext _context;

        public RepositoryRepository(RepositoryDbContext context)
        {
            _context = context;
        }

        public async Task<Repository> GetRepositoryById(int repositoryId)
        {
            return await _context.Repositories.FirstOrDefaultAsync(r => r.Id == repositoryId);
        }
    }
}