using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjInv.Application.UseCases.Investidor.Commands;

namespace ProjInv.API.Controllers
{
    [ApiController]
    [Route("api/Investidor")]
    public class InvestidorController : Controller
    {
        private readonly IMediator _mediator;
        private readonly Serilog.ILogger _logger;

        public InvestidorController(IMediator mediator, Serilog.ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CriarInvestidor([FromBody] CriarInvestidorCommand request)
        {
            try
            {
                var result = await _mediator.Send(request, CancellationToken.None);
                return Ok(new
                {
                    hasError = false,
                    message = "Investidor criado com sucesso.",
                    data = result
                });
            }
            catch (ArgumentException ex)
            {
                _logger.Warning(ex, "Erro de validacao ao criar investidor");
                return BadRequest(new
                {
                    hasError = true,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao criar investidor");
                return StatusCode(500, new
                {
                    hasError = true,
                    message = "Erro interno ao processar a requisicao.",
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvestidores([FromQuery] GetAllInvestidorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(request, cancellationToken);
                return Ok(new
                {
                    hasError = false,
                    message = "Investidores listados com sucesso.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao listar investidores");
                return StatusCode(500, new
                {
                    hasError = true,
                    message = "Erro interno ao processar a requisicao."
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvestidor([FromBody] UpdateInvestidorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(request, cancellationToken);
                return Ok(new
                {
                    hasError = false,
                    message = "Investidor atualizado com sucesso.",
                    data = result
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warning(ex, "Investidor {InvestidorId} nao encontrado", request.Id);
                return NotFound(new
                {
                    hasError = true,
                    message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                _logger.Warning(ex, "Erro de validacao ao atualizar investidor {InvestidorId}", request.Id);
                return BadRequest(new
                {
                    hasError = true,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao atualizar investidor {InvestidorId}", request.Id);
                return StatusCode(500, new
                {
                    hasError = true,
                    message = "Erro interno ao processar a requisicao."
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvestidor(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteInvestidorCommand { Id = id }, CancellationToken.None);
                return Ok(new
                {
                    hasError = false,
                    message = "Investidor excluido com sucesso."
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warning(ex, "Investidor {InvestidorId} nao encontrado", id);
                return NotFound(new
                {
                    hasError = true,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao excluir investidor {InvestidorId}", id);
                return StatusCode(500, new
                {
                    hasError = true,
                    message = "Erro interno ao processar a requisicao."
                });
            }
        }
    }
}
