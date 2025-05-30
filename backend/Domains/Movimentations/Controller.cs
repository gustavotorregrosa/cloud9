using backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Domains.Movimentations
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MovimentationsController : ControllerBase
    {
        private readonly IMovimentationService _service;
        private readonly WebSocketService _webSocketService;

        public MovimentationsController(IMovimentationService service, WebSocketService webSocketService)
        {
            _webSocketService = webSocketService;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductId([FromRoute] Guid productId)
        {
            Console.WriteLine($"Fetching movimentations for product ID: {productId}");
            var result = await _service.GetStockPositionOverTime(productId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MovimentationDtoIn dto)
        {
            var result = await _service.AddAsync(dto);
            _webSocketService.BroadcastMessage("refresh-movimentations");
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MovimentationDtoIn dto)
        {
            var result = await _service.UpdateAsync(dto, id);
            _webSocketService.BroadcastMessage("refresh-movimentations");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            _webSocketService.BroadcastMessage("refresh-movimentations");
            return Ok(result);
        }
    }
}
