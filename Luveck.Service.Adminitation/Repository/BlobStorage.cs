using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Luveck.Service.Administration.Repository.IRepository;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository
{
    public class BlobStorage
    {
        public async Task<List<string>> GetAllDocuments(string connectionString, string containerName)
        {
            var container = BlobExtensions.GetContainer(connectionString, containerName);

            if (!await container.ExistsAsync())
            {
                return new List<string>();
            }

            List<string> blobs = new();

            await foreach (BlobItem blobItem in container.GetBlobsAsync())
            {
                blobs.Add(blobItem.Name);
            }
            return blobs;
        }
        public async Task<string> UploadDocument(string connectionString, string containerName, string fileName, Stream fileContent, string contentType)
        {
            try
            {
                var container = BlobExtensions.GetContainer(connectionString, containerName);
                if (!await container.ExistsAsync())
                {
                    BlobServiceClient blobServiceClient = new(connectionString);
                    await blobServiceClient.CreateBlobContainerAsync(containerName);
                    container = blobServiceClient.GetBlobContainerClient(containerName);
                }

                var bobclient = container.GetBlobClient(fileName);
                if (!bobclient.Exists())
                {
                    fileContent.Position = 0;
                    var blobHttpHeader = new BlobHttpHeaders { ContentType = contentType };
                    var uploadedBlob = await bobclient.UploadAsync(fileContent, new BlobUploadOptions { HttpHeaders = blobHttpHeader });

                    return "C";
                }
                else
                {
                    fileContent.Position = 0;
                    await bobclient.UploadAsync(fileContent, overwrite: true);
                    return "O";
                }
            }
            catch (System.Exception ex)
            {
                throw new FileNotFoundException();
            }

        }
        public async Task<Stream> GetDocument(string connectionString, string containerName, string fileName)
        {
            var container = BlobExtensions.GetContainer(connectionString, containerName);
            if (await container.ExistsAsync())
            {
                var blobClient = container.GetBlobClient(fileName);
                if (blobClient.Exists())
                {
                    var content = await blobClient.DownloadStreamingAsync();
                    return content.Value.Content;
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
            else
            {
                throw new FileNotFoundException();
            }

        }
        public async Task<bool> DeleteDocument(string connectionString, string containerName, string fileName)
        {
            var container = BlobExtensions.GetContainer(connectionString, containerName);
            if (!await container.ExistsAsync())
            {
                return false;
            }

            var blobClient = container.GetBlobClient(fileName);

            if (await blobClient.ExistsAsync())
            {
                await blobClient.DeleteIfExistsAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public static class BlobExtensions
    {
        public static BlobContainerClient GetContainer(string connectionString, string containerName)
        {
            BlobServiceClient blobServiceClient = new(connectionString);
            return blobServiceClient.GetBlobContainerClient(containerName);
        }
    }
}
