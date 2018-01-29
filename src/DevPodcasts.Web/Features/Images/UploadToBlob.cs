using MediatR;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using System.IO;

namespace DevPodcasts.Web.Features.Images
{
    public class UploadToBlob
    {
        public class Command : IRequest<string>
        {
            public byte[] Bytes { get; set; }
            public string BlobReference { get; set; }
            public string ContainerName { get; set; }
            public string ContentType { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, string>
        {
            public string Handle(Command message)
            {
                var connectionString = ConfigurationManager.AppSettings["DevPodcastsStorageConnectionString"];
                var storageAccount = CloudStorageAccount.Parse(connectionString);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference(message.ContainerName);
                var blockBlob = container.GetBlockBlobReference(message.BlobReference);
                blockBlob.Properties.ContentType = message.ContentType;

                blockBlob.UploadFromStream(new MemoryStream(message.Bytes));

                return blockBlob.StorageUri.PrimaryUri.ToString();
            }
        }
    }
}