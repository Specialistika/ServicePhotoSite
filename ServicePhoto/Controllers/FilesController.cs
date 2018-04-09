namespace UploadMVC.WebShow.Controllers
{
	using System;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Threading.Tasks;
	using System.Web.Http;
	using ChuckUpload.WebApi.Context;
	using ChuckUpload.WebApi.FileSystem;
	using ChuckUpload.WebApi.Flow;
	using Constructors.PathString;
	using Constructor.UnZipFileNative;

	[RoutePrefix("folders")]
	public class FilesController : ApiController
	{
		private readonly Flow _flow;
		private readonly FileSystem _fileSystem;
		private readonly string _folderName = VariableConfig.MinePath;
		//private readonly string root = Path.Combine(Path.GetTempPath(), "folders");

		public FilesController()
        {
            _fileSystem = new FileSystem
            {
                GetFilePathFunc = filePath => $"{_folderName}/{filePath}"
			};

            _flow = new Flow(_fileSystem);
        }

        [Route("{*filePath}")]
        public async Task<HttpResponseMessage> GetFile(string filePath)
        {
			if (string.IsNullOrWhiteSpace(filePath))
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid path");
			}

			if (!await _fileSystem.ExistsAsync(filePath).ConfigureAwait(false))
			{
				return Request.CreateErrorResponse(HttpStatusCode.NotFound, "File not found");
			}

			var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(await _fileSystem.OpenReadAsync(filePath).ConfigureAwait(false));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = filePath.Substring(filePath.LastIndexOf('/') + 1)
            };

            return response;
        }

		[Route("uploads/{folderName}"), AcceptVerbs("GET")]
		public async Task<HttpResponseMessage> Get(string folderName)
        {
			var context = CreateContext(folderName);

            return await _flow.HandleRequest(context).ConfigureAwait(false);
        }



		[Route("uploads/{folderName}"), AcceptVerbs("POST")]
		public async Task<HttpResponseMessage> Post(string folderName)
		{
			var context = CreateContext(folderName);
			var result = await _flow.HandleRequest(context).ConfigureAwait(false);

			string NameFile = result.Content.ReadAsStringAsync().Result.Replace("/", "\\").Substring(1);
			string NameFolder = NameFile.Substring(0,  NameFile.IndexOf("\\"));
			string pathNameFile = $"{ _folderName }{NameFile.Substring(0, NameFile.Length - 1)}";

			UnZipNative.InteredArcive(pathNameFile, NameFolder);

			return result;
		}

		private FlowRequestContext CreateContext(string folderName) => new FlowRequestContext(Request)
		{
			GetChunkFileNameFunc = parameters => string.Format("{1}_{0}.chunk",
				parameters.FlowIdentifier,
				parameters.FlowChunkNumber.Value.ToString().PadLeft(8, '0')),
			GetChunkPathFunc = parameters => $"{folderName}/chunks/{parameters.FlowIdentifier}",
			GetFileNameFunc = parameters => parameters.FlowFilename,
			GetFilePathFunc = parameters => folderName,
			GetTempFileNameFunc = filePath => $"file_{Guid.NewGuid()}.tmp",
			GetTempPathFunc = () => $"{folderName}/temp",
			MaxFileSize = ulong.MaxValue
		};
	}
}