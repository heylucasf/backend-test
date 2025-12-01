using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using ProjInv.Application.UseCases.Investidor.Commands;
using ProjInv.Application.UseCases.Investidor.Handles;
using ProjInv.Domain.Entities;
using ProjInv.Domain.Interfaces;
using Serilog;

namespace ProjInv.Tests.Application
{
    public class UpdateInvestidorHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IValidator<UpdateInvestidorCommand>> _validatorMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly UpdateInvestidorHandler _handler;

        public UpdateInvestidorHandlerTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _validatorMock = new Mock<IValidator<UpdateInvestidorCommand>>();
            _loggerMock = new Mock<ILogger>();
            _handler = new UpdateInvestidorHandler(_uowMock.Object, _validatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Deve_Atualizar_Investidor_Com_Sucesso()
        {
            var investidorId = Guid.NewGuid();
            var investidor = new Investidor("Lucas");
            
            var command = new UpdateInvestidorCommand(investidorId, "Lucas Updated");

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _uowMock.Setup(u => u.Investidores.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(investidor);

            _uowMock.Setup(u => u.Investidores.UpdateAsync(It.IsAny<Investidor>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _uowMock.Setup(u => u.CommitAsync())
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Nome.Should().Be(command.Nome);
            
            _uowMock.Verify(u => u.Investidores.UpdateAsync(It.IsAny<Investidor>(), It.IsAny<CancellationToken>()), Times.Once);
            _uowMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_Investidor_Nao_Existir_Update()
        {
            var command = new UpdateInvestidorCommand(Guid.NewGuid(), "Lucas Updated");

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _uowMock.Setup(u => u.Investidores.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Investidor?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
