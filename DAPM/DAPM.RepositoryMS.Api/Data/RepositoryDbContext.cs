﻿using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using Microsoft.EntityFrameworkCore;

/// <author>Nicolai Veiglin Arends</author>
/// <author>Tamás Drabos</author>
namespace DAPM.RepositoryMS.Api.Data
{
    public class RepositoryDbContext : DbContext
    {

        ILogger<RepositoryDbContext> _logger;

        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options, ILogger<RepositoryDbContext> logger) : base (options)
        {
            _logger = logger;
            InitializeDatabase();
        }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<Models.PostgreSQL.File>  Files { get; set; }
        public DbSet<Pipeline> Pipelines { get; set; }

        public DbSet<PipelineExecution> PipelineExecutions { get; set; }
        public DbSet<Operator> Operators { get; set; }

        public void InitializeDatabase()
        {
            if (Database.GetPendingMigrations().Any())
            {
                Database.EnsureDeleted();
                Database.Migrate();

                SaveChanges();
            }
        }
    }
}
