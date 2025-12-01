using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using ProjInv.Application.UseCases.Investimento.Commands;
using ProjInv.Application.UseCases.Investimento.Handles;
using ProjInv.Domain.Entities;
using ProjInv.Domain.Interfaces;

namespace ProjInv.Tests.Application
{
    public class ListarInvestimentosHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IValidator<ListarInvestimentosCommand>> _validatorMock;
        private readonly ListarInvestimentosHandler _handler;

        public ListarInvestimentosHandlerTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _validatorMock = new Mock<IValidator<ListarInvestimentosCommand>>();
            _handler = new ListarInvestimentosHandler(_uowMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task Deve_Listar_Investimentos_Com_Sucesso()
        {
            var command = new ListarInvestimentosCommand { InvestidorId = Guid.NewGuid(), Page = 1, PageSize = 10 };
            var investimento = new Investimento(command.InvestidorId, 1000m, DateTime.UtcNow);
            var investimentos = new List<Investimento> { investimento };

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _uowMock.Setup(u => u.Investimentos.GetByInvestidorIdAsync(command.InvestidorId, command.Page, command.PageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(investimentos);

            _uowMock.Setup(u => u.Investimentos.CountByInvestidorIdAsync(command.InvestidorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Investimentos.Should().HaveCount(1);
            result.TotalCount.Should().Be(1);
        }
    }
}
