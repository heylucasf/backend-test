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
    public class DeleteInvestidorHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IValidator<DeleteInvestidorCommand>> _validatorMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly DeleteInvestidorHandler _handler;

        public DeleteInvestidorHandlerTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _validatorMock = new Mock<IValidator<DeleteInvestidorCommand>>();
            _loggerMock = new Mock<ILogger>();
            _handler = new DeleteInvestidorHandler(_uowMock.Object, _validatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Deve_Deletar_Investidor_Com_Sucesso()
        {
            var command = new DeleteInvestidorCommand { Id = Guid.NewGuid() };
            var investidor = new Investidor("Lucas");

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _uowMock.Setup(u => u.Investidores.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(investidor);

            _uowMock.Setup(u => u.Investidores.DeleteAsync(command.Id, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _uowMock.Setup(u => u.CommitAsync())
                .Returns(Task.CompletedTask);

            await _handler.Handle(command, CancellationToken.None);

            _uowMock.Verify(u => u.Investidores.DeleteAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
            _uowMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_Investidor_Nao_Existir_Delete()
        {
            var command = new DeleteInvestidorCommand { Id = Guid.NewGuid() };

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _uowMock.Setup(u => u.Investidores.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Investidor?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
