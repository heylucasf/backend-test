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
    public class CriarInvestidorHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IValidator<CriarInvestidorCommand>> _validatorMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly CriarInvestidorHandler _handler;

        public CriarInvestidorHandlerTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _validatorMock = new Mock<IValidator<CriarInvestidorCommand>>();
            _loggerMock = new Mock<ILogger>();
            _handler = new CriarInvestidorHandler(_uowMock.Object, _validatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Deve_Criar_Investidor_Com_Sucesso()
        {
            var command = new CriarInvestidorCommand { Nome = "Lucas" };

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _uowMock.Setup(u => u.Investidores.AddAsync(It.IsAny<Investidor>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _uowMock.Setup(u => u.CommitAsync())
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Nome.Should().Be(command.Nome);
            result.Id.Should().NotBeEmpty();

            _uowMock.Verify(u => u.Investidores.AddAsync(It.IsAny<Investidor>(), It.IsAny<CancellationToken>()), Times.Once);
            _uowMock.Verify(u => u.CommitAsync(), Times.Once);
        }
    }
}
