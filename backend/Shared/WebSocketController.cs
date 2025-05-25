using Microsoft.AspNetCore.Mvc;
using backend.Shared.Authentication;

namespace backend.Shared
{
    // [ApiController]
    // [Route("api/[controller]")]
    public class WebSocketController : ControllerBase
    {
        private readonly WebSocketService _webSocketService;

        private readonly IAuthenticationService _authenticationService;

        public WebSocketController(WebSocketService webSocketService, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _webSocketService = webSocketService;
        }

        [HttpGet("/ws")]
        public async Task Get()
        {
            
            var queryParams = HttpContext.Request.Query;
            var authParam = queryParams["auth"].ToString();
           
            try
            {
                if (!string.IsNullOrEmpty(authParam))
                {
                    var token = authParam;
                    var user = await _authenticationService.ValidateToken(token);

                    if (user == null)
                    {
                        HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        Console.WriteLine("User not found or token is invalid.");

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating token: {ex.Message}");
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await _webSocketService.HandleWebSocketConnection(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

    }
}