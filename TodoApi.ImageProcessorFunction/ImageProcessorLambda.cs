using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;

namespace TodoApi.ImageProcessorFunction
{
    public class ImageProcessorLambda
    {
        [FunctionName("ImageProcessorLambda")]
        public async Task Run(
            [BlobTrigger("images/{name}", Connection = "BlobStorageConnectionString")]Stream myBlob,
            [Blob("imagesresized/{name}", FileAccess.Write, Connection = "BlobStorageConnectionString")]Stream outputBlob,
            string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            using (var image = await Image.LoadAsync(myBlob))
            {
                image.Mutate(x => x.Resize(500, 500, KnownResamplers.Lanczos3));
                var metadata = image.Metadata;
                image.Save(outputBlob, metadata.DecodedImageFormat);
            }
            log.LogInformation($"Image {name} has been successfully resized and folded into :imagesresized/{name}");
        }
    }
}
