using System.Net;
using ElasticSearch.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticSearch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {

        [NonAction]
        public IActionResult CreateActionResult<T>(ResponseDto<T> responseDto)
        {
            if (responseDto.HttpStatusCode == HttpStatusCode.NoContent)
                return new ObjectResult(null) { StatusCode = responseDto.HttpStatusCode.GetHashCode() };

            return new ObjectResult(responseDto) { StatusCode = responseDto.HttpStatusCode.GetHashCode() };
        }
    }
}
