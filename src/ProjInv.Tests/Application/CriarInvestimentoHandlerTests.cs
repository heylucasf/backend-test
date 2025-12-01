using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using ProjInv.Application.UseCases.Investimento.Commands;
using ProjInv.Application.UseCases.Investimento.Handles;
using ProjInv.Domain.Entities;
using ProjInv.Domain.Interfaces;
using Serilog;

namespace ProjInv.Tests.Application
{
    public class CriarInvestimentoHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IValidator<CriarInvestimentoCommand>> _validatorMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly CriarInvestimentoHandler _handler;

        public CriarInvestimentoHandlerTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _validatorMock = new Mock<IValidator<CriarInvestimentoCommand>>();
            _loggerMock = new Mock<ILogger>();
            _handler = new CriarInvestimentoHandler(_uowMock.Object, _validatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Deve_Criar_Investimento_Com_Sucesso()
        {
            var command = new CriarInvestimentoCommand
            {
                InvestidorId = Guid.NewGuid(),
                ValorInicial = 1000m,
                DataCriacao = DateTime.UtcNow.Date
            };

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _uowMock.Setup(u => u.Investidores.GetByIdAsync(command.InvestidorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Investidor("Teste"));

            _uowMock.Setup(u => u.Investimentos.AddAsync(It.IsAny<Investimento>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _uowMock.Setup(u => u.CommitAsync())
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.ValorInicial.Should().Be(command.ValorInicial);
            result.InvestidorId.Should().Be(command.InvestidorId);

            _uowMock.Verify(u => u.Investimentos.AddAsync(It.IsAny<Investimento>(), It.IsAny<CancellationToken>()), Times.Once);
            _uowMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_Investidor_Nao_Existir()
        {
            var command = new CriarInvestimentoCommand
            {
                InvestidorId = Guid.NewGuid(),
                ValorInicial = 1000m,
                DataCriacao = DateTime.UtcNow.Date
            };

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _uowMock.Setup(u => u.Investidores.GetByIdAsync(command.InvestidorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Investidor?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
