using Microsoft.AspNetCore.Mvc;

namespace Academy.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HandleError(int statusCode)
        {
            return statusCode switch
            {
                401 => View("Error401"),
                403 => View("Error403"),
                404 => View("Error404"),
                _   => View("Error500")
            };
        }
    }
}
