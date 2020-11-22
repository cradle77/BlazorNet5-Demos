using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using BlazorNet5.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace BlazorNet5.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private string _connectionString;
        private string _accountName;
        private string _key;

        public PhotosController(IConfiguration configuration)
        {
            _connectionString = configuration.GetSection("Blob:ConnectionString").Value;
            _accountName = configuration.GetSection("Blob:accountName").Value;
            _key = configuration.GetSection("Blob:key").Value;
        }

        [HttpPost]
        public IActionResult Create(FileUploadRequest request)
        {
            if (request == null)
                return this.BadRequest();

            AccountSasBuilder sas = new AccountSasBuilder
            {
                // Allow access to blobs
                Services = AccountSasServices.Blobs,

                // Allow access to the service level APIs
                ResourceTypes = AccountSasResourceTypes.Object,

                // Access expires in 1 hour!
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
                StartsOn = DateTimeOffset.UtcNow.AddSeconds(-30)
            };

            sas.SetPermissions(AccountSasPermissions.Write | AccountSasPermissions.Create | AccountSasPermissions.Delete);

            StorageSharedKeyCredential credential = new StorageSharedKeyCredential(_accountName, _key);

            var container = new BlobContainerClient(_connectionString, "photos");

            var blob = container.GetBlobClient(request.FileName);

            var result = new UriBuilder(blob.Uri);
            result.Query = sas.ToSasQueryParameters(credential).ToString();

            return this.Ok(result.Uri.ToString());
        }
    }
}
