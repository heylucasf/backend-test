using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjInv.Application.UseCases.Investimento.Commands;

namespace ProjInv.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvestimentoController : Controller
    {
        private readonly IMediator _mediator;
        private readonly Serilog.ILogger _logger;

        public InvestimentoController(IMediator mediator, Serilog.ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CriarInvestimento([FromBody] CriarInvestimentoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(request, cancellationToken);
                return Ok(new
                {
                    hasError = false,
                    message = "Investimento criado com sucesso.",
                    data = result
                });
            }
            catch (ArgumentException ex)
            {
                _logger.Warning(ex, "Erro de validacao ao criar investimento");
                return BadRequest(new
                {
                    hasError = true,
                    message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warning(ex, "Investidor nao encontrado");
                return NotFound(new
                {
                    hasError = true,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao criar investimento");
                return StatusCode(500, new
                {
                    hasError = true,
                    message = "Erro interno ao processar a requisicao."
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvestimentoById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(new VisualizarInvestimentoCommand { Id = id }, cancellationToken);
                return Ok(new
                {
                    hasError = false,
                    message = "Investimento encontrado com sucesso.",
                    data = result
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warning(ex, "Investimento {InvestimentoId} nao encontrado", id);
                return NotFound(new
                {
                    hasError = true,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao buscar investimento {InvestimentoId}", id);
                return StatusCode(500, new
                {
                    hasError = true,
                    message = "Erro interno ao processar a requisicao."
                });
            }
        }

        [HttpPost("{id}/retirar")]
        public async Task<IActionResult> RetirarInvestimento(Guid id, [FromBody] RetirarInvestimentoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (id != request.InvestimentoId && request.InvestimentoId != Guid.Empty)
                {
                    return BadRequest(new
                    {
                        hasError = true,
                        message = "ID do investimento na URL difere do ID no corpo da requisicao."
                    });
                }

                request.InvestimentoId = id;
                var result = await _mediator.Send(request, cancellationToken);
                return Ok(new
                {
                    hasError = false,
                    message = "Retirada realizada com sucesso.",
                    data = result
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warning(ex, "Investimento {InvestimentoId} nao encontrado", id);
                return NotFound(new
                {
                    hasError = true,
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Warning(ex, "Operacao invalida ao retirar investimento {InvestimentoId}", id);
                return BadRequest(new
                {
                    hasError = true,
                    message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                _logger.Warning(ex, "Erro de validacao ao retirar investimento {InvestimentoId}", id);
                return BadRequest(new
                {
                    hasError = true,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao processar retirada do investimento {InvestimentoId}", id);
                return StatusCode(500, new
                {
                    hasError = true,
                    message = "Erro interno ao processar a requisicao."
                });
            }
        }

        [HttpGet("/api/investidor/{investidorId}/investimentos")]
        public async Task<IActionResult> ListarInvestimentos(Guid investidorId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            try
            {
                var command = new ListarInvestimentosCommand
                {
                    InvestidorId = investidorId,
                    Page = page,
                    PageSize = pageSize
                };
                var result = await _mediator.Send(command, cancellationToken);
                return Ok(new
                {
                    hasError = false,
                    message = "Investimentos listados com sucesso.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao listar investimentos do investidor {InvestidorId}", investidorId);
                return StatusCode(500, new
                {
                    hasError = true,
                    message = "Erro interno ao processar a requisicao."
                });
            }
        }
    }
}
