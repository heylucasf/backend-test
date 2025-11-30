using FluentValidation;
using MediatR;
using ProjInv.Application.DTOs.Responses.Investidor;
using ProjInv.Application.UseCases.Investidor.Commands;
using ProjInv.Domain.Interfaces;
using Serilog;

namespace ProjInv.Application.UseCases.Investidor.Handles
{
    public class UpdateInvestidorHandler : IRequestHandler<UpdateInvestidorCommand, InvestidorResponseDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<UpdateInvestidorCommand> _validator;
        private readonly ILogger _logger;

        public UpdateInvestidorHandler(
            IUnitOfWork uow, 
            IValidator<UpdateInvestidorCommand> validator,
            ILogger logger)
        {
            _uow = uow;
            _validator = validator;
            _logger = logger;
        }

        public async Task<InvestidorResponseDto> Handle(UpdateInvestidorCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.Error("Falha ao validar UpdateInvestidorCommand: {Errors}", errors);
                throw new ArgumentException(errors);
            }

            await _uow.BeginTransactionAsync();

            try
            {
                var investidor = await _uow.Investidores.GetByIdAsync(request.Id, cancellationToken);
                if (investidor == null)
                {
                    _logger.Error("Investidor com Id {Id} nao encontrado para atualizacao.", request.Id);
                    throw new KeyNotFoundException("Investidor nao encontrado.");
                }

                investidor.UpdateNome(request.Nome);
                await _uow.Investidores.UpdateAsync(investidor, cancellationToken);
                await _uow.CommitAsync();

                return new InvestidorResponseDto
                {
                    Id = investidor.Id,
                    Nome = investidor.Nome
                };
            }
            catch
            {
                await _uow.RollbackAsync();
                _logger.Error("Erro ao atualizar Investidor com Id {Id}.", request.Id);
                throw;
            }
        }
    }
}
