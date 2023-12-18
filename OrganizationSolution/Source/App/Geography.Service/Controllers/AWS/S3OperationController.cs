 using EnsureThat;
using Framework.Business;
using Framework.Business.ServiceProvider.Storage;
using Framework.Configuration.Models;
using Framework.Service;
using Geography.Business.Country; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations; 
using System.IO;
using System.Threading.Tasks;

namespace Geography.Service.Controllers.AWS
{
    /// <summary>
    /// S3OperationController
    /// </summary>
    [Route("api/s3/[controller]")]
    [ApiController]
    [Produces(SupportedContentTypes.Json, SupportedContentTypes.Xml)]
    [ApiExplorerSettings(GroupName = ApiConstants.ApiVersion)]
    public class S3OperationController : ControllerBase
    {

        private readonly IStorageManager<AmazonS3ConfigurationOptions> _manager;
        private readonly AmazonS3ConfigurationOptions _amazonS3Configuration;

        /// <summary>
        /// S3OperationController Constructor
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="options"></param>
        public S3OperationController(IStorageManager<AmazonS3ConfigurationOptions> manager, ApplicationOptions options)
        {
            EnsureArg.IsNotNull(manager, nameof(manager));
            _manager = manager;
            _amazonS3Configuration = options.amazons3ConfigurationOptions;
        }
        /// <summary>
        /// Upload
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>

        [HttpPost(nameof(Upload))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            using (Stream fileContent = file.OpenReadStream())
            {
                var result = await _manager.UploadFileAsync(_amazonS3Configuration, file.FileName, fileContent).ConfigureAwait(false);
                return Ok(result);
            }
        }
        /// <summary>
        /// GetFiles
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(GetFiles))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFiles()
        {
            var result = await _manager.GetFiles(_amazonS3Configuration).ConfigureAwait(false);
            return Ok(result);
        }
        /// <summary>
        /// ReadFile
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpGet(nameof(ReadFile))]
        [ProducesResponseType(typeof(ManagerResponse<CountryErrorCode>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ManagerResponse<CountryErrorCode>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadFile([Required] string filePath)
        {
            var result = await _manager.ReadFile(_amazonS3Configuration, filePath).ConfigureAwait(false);
            return Ok(result);
        }
        /// <summary>
        /// ReadFiles
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(ReadFiles))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadFiles()
        {
            var result = await _manager.ReadFiles(_amazonS3Configuration).ConfigureAwait(false);
            return Ok(result);
        }
    }
}
