#r "Microsoft.WindowsAzure.Storage"
#r "Newtonsoft.Json"

using System;
using System.Configuration;
using System.Net;
using System.Text;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

// create,set the UploadStorageAccessKey in the Application settings, also add your url to the CORS
public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    HttpStatusCode result;

    result = HttpStatusCode.BadRequest;

    var provider = new MultipartMemoryStreamProvider();
    await req.Content.ReadAsMultipartAsync(provider);
    var file = provider.Contents.First();
    var fileInfo = file.Headers.ContentDisposition;
    var fileData = await file.ReadAsByteArrayAsync();

    if(fileData != null)
    {
        string name;

        name = Guid.NewGuid().ToString("n") + fileInfo.FileName.Replace("\"","");

        await CreateBlob(name, fileData, log);
        result = HttpStatusCode.OK;
        var jsonToReturn = JsonConvert.SerializeObject(new {name = name});

        return new HttpResponseMessage(result) {
            Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
        };
    }

    return req.CreateResponse(result, "");
}

private async static Task CreateBlob(string name, byte[] data, TraceWriter log)
{
    string connectionString;
    CloudStorageAccount storageAccount;
    CloudBlobClient client;
    CloudBlobContainer container;
    CloudBlockBlob blob;

    connectionString = ConfigurationManager.AppSettings["UploadStorageAccessKey"];
    storageAccount = CloudStorageAccount.Parse(connectionString);

    client = storageAccount.CreateCloudBlobClient();
    
    container = client.GetContainerReference("testing123");
   
    await container.CreateIfNotExistsAsync();
    
    blob = container.GetBlockBlobReference(name);
    if (name.EndsWith(".jpeg") || name.EndsWith(".jpg"))
    {
        blob.Properties.ContentType = "application/jpeg";
    }

    using (Stream stream = new MemoryStream(data))
    {
        await blob.UploadFromStreamAsync(stream);
    }
}