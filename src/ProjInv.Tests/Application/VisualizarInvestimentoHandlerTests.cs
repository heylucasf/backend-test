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
    public class VisualizarInvestimentoHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IValidator<VisualizarInvestimentoCommand>> _validatorMock;
        private readonly VisualizarInvestimentoHandler _handler;

        public VisualizarInvestimentoHandlerTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _validatorMock = new Mock<IValidator<VisualizarInvestimentoCommand>>();
            _handler = new VisualizarInvestimentoHandler(_uowMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task Deve_Visualizar_Investimento_Com_Sucesso()
        {
            var command = new VisualizarInvestimentoCommand { Id = Guid.NewGuid() };
            var investimento = new Investimento(Guid.NewGuid(), 1000m, DateTime.UtcNow);
            
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _uowMock.Setup(u => u.Investimentos.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(investimento);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.ValorInicial.Should().Be(1000m);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_Investimento_Nao_Existir()
        {
            var command = new VisualizarInvestimentoCommand { Id = Guid.NewGuid() };

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _uowMock.Setup(u => u.Investimentos.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Investimento?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
