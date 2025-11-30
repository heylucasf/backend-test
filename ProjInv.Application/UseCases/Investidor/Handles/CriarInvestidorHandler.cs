using MediatR;
using ProjInv.Domain.Entities;
using ProjInv.Domain.Interfaces;

using FluentValidation;
using ProjInv.Application.DTOs.Responses.Investidor;
using ProjInv.Application.UseCases.Investidor.Commands;
using Serilog;

namespace ProjInv.Application.UseCases.Investidor.Handles
{
    public class CriarInvestidorHandler : IRequestHandler<CriarInvestidorCommand, InvestidorResponseDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<CriarInvestidorCommand> _validator;
        private readonly ILogger _logger;

        public CriarInvestidorHandler(
            IUnitOfWork uow, 
            IValidator<CriarInvestidorCommand> validator,
            ILogger logger)
        {
            _uow = uow;
            _validator = validator;
            _logger = logger;
        }

        public async Task<InvestidorResponseDto> Handle(CriarInvestidorCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.Error("Falha ao validar o CriarInvestidorCommand: {Errors}", errors);
                throw new ArgumentException(errors);
            }

            await _uow.BeginTransactionAsync();

            try
            {
                var investidor = new Domain.Entities.Investidor(request.Nome);
                await _uow.Investidores.AddAsync(investidor, cancellationToken);
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
                _logger.Error("Erro ao criar investidor");
                throw;
            }
        }
    }
}
