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
    public class RetirarInvestimentoHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IValidator<RetirarInvestimentoCommand>> _validatorMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly RetirarInvestimentoHandler _handler;

        public RetirarInvestimentoHandlerTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _validatorMock = new Mock<IValidator<RetirarInvestimentoCommand>>();
            _loggerMock = new Mock<ILogger>();
            _handler = new RetirarInvestimentoHandler(_uowMock.Object, _validatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Deve_Realizar_Retirada_Com_Sucesso()
        {
            var investimentoId = Guid.NewGuid();
            var investidorId = Guid.NewGuid();
            var dataCriacao = DateTime.UtcNow.AddMonths(-6).Date;
            var investimento = new Investimento(investidorId, 1000m, dataCriacao);

            var command = new RetirarInvestimentoCommand
            {
                InvestimentoId = investimentoId,
                DataRetirada = DateTime.UtcNow.Date
            };

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _uowMock.Setup(u => u.Investimentos.GetByIdAsync(command.InvestimentoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(investimento);

            _uowMock.Setup(u => u.Retiradas.AddAsync(It.IsAny<Retirada>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _uowMock.Setup(u => u.CommitAsync())
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.ValorLiquido.Should().BeGreaterThan(0);
            
            _uowMock.Verify(u => u.Retiradas.AddAsync(It.IsAny<Retirada>(), It.IsAny<CancellationToken>()), Times.Once);
            _uowMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Permitir_Retirada_Se_Ja_Retirado()
        {
            var investimentoId = Guid.NewGuid();
            var investidorId = Guid.NewGuid();
            var dataCriacao = DateTime.UtcNow.AddMonths(-6).Date;
            var investimento = new Investimento(investidorId, 1000m, dataCriacao);
            
            var retirada = new Retirada(investimentoId, DateTime.UtcNow, 1000, 0, 1000);
            typeof(Investimento).GetProperty("Retirada")!.SetValue(investimento, retirada);

            var command = new RetirarInvestimentoCommand
            {
                InvestimentoId = investimentoId,
                DataRetirada = DateTime.UtcNow.Date
            };

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _uowMock.Setup(u => u.Investimentos.GetByIdAsync(command.InvestimentoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(investimento);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
