using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Application.Common.Options.Azure;
using TodoApi.Core.Entities;

namespace TodoApi.Infrastructure.Repositories
{
    public class ApplicationContenxt : DbContext
    {
        private readonly AzureCosmosDbOptions _azureCosmosDbOptions;
        public DbSet<Todo> Todos { get; set; }
        public ApplicationContenxt(IOptions<AzureCosmosDbOptions> azureCosmosDbOptions)
        {
            _azureCosmosDbOptions = azureCosmosDbOptions.Value ?? throw new ArgumentNullException($"{nameof(AzureCosmosDbOptions)} is null");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Todo>()
             .ToContainer(_azureCosmosDbOptions.ContainerName)
             .HasPartitionKey(t => t.Id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(_azureCosmosDbOptions.ConnectionString, _azureCosmosDbOptions.DatabaseName);
        }
    }
}
