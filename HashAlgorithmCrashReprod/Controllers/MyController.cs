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
        public async Task<IActionResult> RunTestAsync()
        {
            string userId = Guid.NewGuid().ToString();
            string accessToken = MyService.GenerateAccessToken(userId);

            await MyService.ValidateAccessTokenAsync(accessToken);

            return Ok();
        }
    }
}