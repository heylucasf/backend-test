using MediatR;
using ProjInv.Application.DTOs.Responses;
using ProjInv.Domain.Interfaces;

using FluentValidation;
using ProjInv.Application.DTOs.Responses.Investidor;
using ProjInv.Application.UseCases.Investidor.Commands;
using Serilog;

namespace ProjInv.Application.UseCases.Investidor
{
    public class GetAllInvestidorHandler : IRequestHandler<GetAllInvestidorCommand, GetAllInvestidorResponseDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<GetAllInvestidorCommand> _validator;
        private readonly ILogger _logger;

        public GetAllInvestidorHandler(
            IUnitOfWork uow, 
            IValidator<GetAllInvestidorCommand> validator,
            ILogger logger)
        {
            _uow = uow;
            _validator = validator;
            _logger = logger;
        }

        public async Task<GetAllInvestidorResponseDto> Handle(GetAllInvestidorCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.Error("Falha ao validar GetAllInvestidorCommand: {Errors}", errors);
                throw new ArgumentException(errors);
            }

            var investidores = await _uow.Investidores.GetAllAsync(request.Page, request.PageSize, cancellationToken);

            return new GetAllInvestidorResponseDto
            {
                Investidores = investidores.Select(x => new InvestidorDto
                {
                    Id = x.Id,
                    Nome = x.Nome
                }).ToList()
            };
        }
    }
}
