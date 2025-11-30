using FluentValidation;
using MediatR;
using ProjInv.Application.UseCases.Investidor.Commands;
using ProjInv.Domain.Interfaces;
using Serilog;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjInv.Application.UseCases.Investidor.Handles
{
    public class DeleteInvestidorHandler : IRequestHandler<DeleteInvestidorCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<DeleteInvestidorCommand> _validator;
        private readonly ILogger _logger;

        public DeleteInvestidorHandler(
            IUnitOfWork uow, 
            IValidator<DeleteInvestidorCommand> validator,
            ILogger logger)
        {
            _uow = uow;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteInvestidorCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.Error("Falha ao validar o DeleteInvestidorCommand: {Errors}", errors);
                throw new ArgumentException(errors);
            }

            await _uow.BeginTransactionAsync();

            try
            {
                var investidor = await _uow.Investidores.GetByIdAsync(request.Id, cancellationToken);
                if (investidor == null)
                {
                    _logger.Error("Investidor não encontrado para Remoção.");
                    throw new KeyNotFoundException("Investidor nao encontrado.");
                }

                await _uow.Investidores.DeleteAsync(request.Id, cancellationToken);
                await _uow.CommitAsync();

                return Unit.Value;
            }
            catch
            {
                await _uow.RollbackAsync();
                _logger.Error("Erro ao deletar o Investidor: {Id}", request.Id);
                throw;
            }
        }
    }
}
