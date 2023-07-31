using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApi.Application.Common.Options.Azure
{
    public class AzureCosmosDbOptions
    {
        public const string SectionName = "AzureCosmosDb";

        public string ConnectionString { get; set; } = default!;
        public string DatabaseName { get; set; } = default!;
        public string ContainerName { get; set; } = default!;
       
            
    }
}
