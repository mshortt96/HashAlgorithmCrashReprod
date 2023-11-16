using HashAlgorithmCrashReprod.Services;
using Microsoft.AspNetCore.Mvc;

namespace HashAlgorithmCrashReprod.Controllers
{
    [ApiController]
    [Route("/")]
    public class MyController : ControllerBase
    {
        private readonly MyService MyService;

        public MyController(MyService myService)
        {
            MyService = myService;
        }

        [HttpGet]
        public IActionResult Home()
        {
            string message = @"Please hit the ""run-test"" endpoint";
            return Ok(message);
        }

        [HttpGet]
        [Route("run-test")]
        public async Task<IActionResult> RunTestAsync([FromQuery]bool deepCloneValidationParams)
        {
            string userId = Guid.NewGuid().ToString();
            string accessToken = MyService.GenerateAccessToken(userId);

            if(deepCloneValidationParams)
            {
                await MyService.ValidateAccessTokenWithDeepClonedParametersAsync(accessToken);
            }

            else
            {
                await MyService.ValidateAccessTokenWithShallowClonedParametersAsync(accessToken);
            }

            return Ok();
        }
    }
}