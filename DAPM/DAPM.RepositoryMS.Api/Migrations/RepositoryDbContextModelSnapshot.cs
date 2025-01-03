﻿// <auto-generated />
using System;
using DAPM.RepositoryMS.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAPM.RepositoryMS.Api.Migrations
{
    [DbContext(typeof(RepositoryDbContext))]
    partial class RepositoryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DAPM.RepositoryMS.Api.Models.PostgreSQL.File", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MongoDbFileId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("DAPM.RepositoryMS.Api.Models.PostgreSQL.Operator", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DockerfileFileId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RepositoryId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SourceCodeFileId")
                        .HasColumnType("uuid");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DockerfileFileId");

                    b.HasIndex("RepositoryId");

                    b.HasIndex("SourceCodeFileId");

                    b.ToTable("Operators");
                });

            modelBuilder.Entity("DAPM.RepositoryMS.Api.Models.PostgreSQL.Pipeline", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PipelineJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RepositoryId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RepositoryId");

                    b.ToTable("Pipelines");
                });

            modelBuilder.Entity("DAPM.RepositoryMS.Api.Models.PostgreSQL.PipelineExecution", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("PipelineId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RepositoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PipelineExecutions");
                });

            modelBuilder.Entity("DAPM.RepositoryMS.Api.Models.PostgreSQL.Repository", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Repositories");
                });

            modelBuilder.Entity("DAPM.RepositoryMS.Api.Models.PostgreSQL.Resource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FileId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RepositoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.HasIndex("RepositoryId");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("DAPM.RepositoryMS.Api.Models.PostgreSQL.Operator", b =>
                {
                    b.HasOne("DAPM.RepositoryMS.Api.Models.PostgreSQL.File", "DockerfileFile")
                        .WithMany()
                        .HasForeignKey("DockerfileFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAPM.RepositoryMS.Api.Models.PostgreSQL.Repository", "Repository")
                        .WithMany()
                        .HasForeignKey("RepositoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAPM.RepositoryMS.Api.Models.PostgreSQL.File", "SourceCodeFile")
                        .WithMany()
                        .HasForeignKey("SourceCodeFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DockerfileFile");

                    b.Navigation("Repository");

                    b.Navigation("SourceCodeFile");
                });

            modelBuilder.Entity("DAPM.RepositoryMS.Api.Models.PostgreSQL.Pipeline", b =>
                {
                    b.HasOne("DAPM.RepositoryMS.Api.Models.PostgreSQL.Repository", "Repository")
                        .WithMany()
                        .HasForeignKey("RepositoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Repository");
                });

            modelBuilder.Entity("DAPM.RepositoryMS.Api.Models.PostgreSQL.Resource", b =>
                {
                    b.HasOne("DAPM.RepositoryMS.Api.Models.PostgreSQL.File", "File")
                        .WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAPM.RepositoryMS.Api.Models.PostgreSQL.Repository", "Repository")
                        .WithMany()
                        .HasForeignKey("RepositoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("File");

                    b.Navigation("Repository");
                });
#pragma warning restore 612, 618
        }
    }
}
