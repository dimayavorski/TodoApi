using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApi.Application.Common.Options.Azure
{
    public class AzureBlobStorageOptions
    {
        public const string SectionName = "AzureBlobStorage";

        public string ServiceUrl { get; set; } = default!;
        public string ContainerName { get; set; } = default!;
    }
}
