using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyListApp.Graph;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MyListApp
{
    public class Add
    {
        private readonly IFileService _fileService;
        private readonly IListService _listService;
        private readonly ListOptions _listOptions;

        public Add(IFileService fileService, IListService listService, IOptions<ListOptions> listOptions)
        {
            _fileService = fileService;
            _listService = listService;
            _listOptions = listOptions.Value;
        }

        [FunctionName("Add")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "postings")] HttpRequest req,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic posting = JsonConvert.DeserializeObject(requestBody);

            var bytes = Convert.FromBase64String((string)posting.photo);
            var fileName = $"{Guid.NewGuid()}.png";
            var path = $"drives/{_listOptions.AssetDriveId}/root:/Lists/{_listOptions.ListId}/{fileName}";
            var driveItem = await _fileService.UploadAsync(path, bytes, "image/png");

            var photoUri = new Uri(driveItem.WebUrl);
            var sharePointColumnImage = new SharePointColumnImage()
            {
                FieldName = "Photo",
                FileName = fileName,
                ServerUrl = driveItem.WebUrl.Substring(0, driveItem.WebUrl.Length - photoUri.LocalPath.Length),
                ServerRelativeUrl = photoUri.LocalPath
            };

            var listItem = new ListItem()
            {
                Fields = new Dictionary<string, string>()
                {
                    { "Photo", JsonConvert.SerializeObject(sharePointColumnImage) },
                    { "GivenName", (string)posting.givenName },
                    { "FamilyName", (string)posting.familyName },
                    { "E_x002d_Mail", (string)posting.email },
                }
            };
            await _listService.CreateItemAsync(_listOptions.SiteId, _listOptions.ListId, listItem);

            return new StatusCodeResult(StatusCodes.Status201Created);
        }
    }
}